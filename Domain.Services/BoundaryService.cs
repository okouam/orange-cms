using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain.Providers;
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
        private readonly IDbContextScope dbContextScope;

        public BoundaryService(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public async Task<IEnumerable<Boundary>> GetAll()
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return await dbContext.Boundaries.ToListAsync();
            }
        }

        public IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string filename)
        {
            var boundaries = GetBoundariesFromZip(filename, nameColumn);
            var repository = new BoundaryRepository(dbContextScope.CreateDbContext());
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
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return await dbContext.Boundaries.FindAsync(id);
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
                    var geometry = (Geometry) featureSet.GetShape(i, true).ToGeometry();

                    yield return new Boundary
                    {
                        Name = featureSet.GetFeature(i).DataRow[nameColumn].ToString(),
                        Shape = DbGeography.FromText(writer.Write(geometry), 4326),
                    };
                }
            }
        }
    }
}
