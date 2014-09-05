using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;
using Ionic.Zip;
using OrangeCMS.Domain;
using OrangeCMS.Domain.Services;
using Codeifier.OrangeCMS.Repositories;

namespace OrangeCMS.Application.Services
{
    public class BoundaryService : IBoundaryService
    {
        public async Task<IEnumerable<Boundary>> GetAll()
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Boundaries.ToListAsync();
            }
        }

        public IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string filename)
        {
            var boundaries = GetBoundariesFromZip(filename, nameColumn);
            var repository = new BoundaryRepository(new DatabaseContext());
            repository.Save(boundaries);
            return boundaries;
        }

        public string ExtractShapefileFromZip(string file)
        {
            if (String.IsNullOrEmpty(file)) throw new ArgumentException(file);

            if (!File.Exists(file)) throw new ArgumentException(file);

            var tempDirectoryName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempDirectory = Directory.CreateDirectory(tempDirectoryName);
            var outputDirectory = tempDirectory.FullName;

            using (var zipFile = ZipFile.Read(file))
            {
                foreach (var entry in zipFile)
                {
                    entry.Extract(tempDirectory.FullName, ExtractExistingFileAction.OverwriteSilently);
                    if (entry.IsDirectory)
                    {
                        outputDirectory = Path.Combine(outputDirectory, entry.FileName);
                    }
                }
            }

            var files = Directory.GetFiles(outputDirectory);

            var shp = files.FirstOrDefault();

            if (shp == null)
            {
                throw new Exception(String.Format("Unable to find a .shp file in the files extracted from {0}.", file));
            }

            return shp;
        }

        public async Task<Boundary> Get(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Boundaries.FindAsync(id);
            }
        }

        public IList<Coordinate> GenerateRandomCoordinatesIn(string shapefile, int numPoints)
        {
            using (var featureSet = FeatureSet.Open(shapefile))
            {
                var merge = featureSet.GetFeature(0);

                for (var i = 1; i < featureSet.NumRows(); i++)
                {
                    try
                    {              
                        merge = merge.Union(featureSet.GetFeature(i));
                    }
                    catch
                    {
                        // TODO: THIS IS WRONG!!! BUT GOOD ENOUGH FOR OUR PURPOSES WHICH IS JUST DEMO DATA AT THIS TIME. 
                        // IF THIS CODE IS EVER USED IN ANGER, --MUST-- BE FIXED. GETTING DUPLICATE POINTS IN THE GEOMETRIES...
                    }
                }

                return RandomGeometry.RandomPoints((Feature) merge, numPoints).Features.Select(x => x.Coordinates.First()).ToList();
            }
        }

        public IEnumerable<Boundary> GetBoundariesFromZip(string file, string nameColumn, int maxBoundaries = int.MaxValue)
        {
            var shp = ExtractShapefileFromZip(file);
            return GetBoundariesFromShapefile(shp, nameColumn, maxBoundaries);
        }

        private IEnumerable<Boundary> GetBoundariesFromShapefile(string file, string nameColumn, int maxBoundaries = int.MaxValue)
        {
            using (var featureSet = FeatureSet.Open(file))
            {
                var dataTable = featureSet.DataTable;

                if (!dataTable.Columns.Contains(nameColumn))
                {
                    throw new Exception(String.Format("The column name {0} is not present in the .dbf file for the shapefile", nameColumn));
                }
                
                var writer = new WktWriter();

                var max = featureSet.NumRows();

                if (max > maxBoundaries) max = maxBoundaries;

                for (var i = 0; i < max; i++)
                {
                    yield return new Boundary
                    {
                        Name = featureSet.GetFeature(i).DataRow[nameColumn].ToString(),
                        WKT = writer.Write((Geometry) featureSet.GetShape(i, true).ToGeometry()),
                    };
                }
            }
        }
    }
}
