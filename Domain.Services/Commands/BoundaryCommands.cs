using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;
using Ionic.Zip;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands
{
    public class BoundaryCommands : IBoundaryCommands
    {
        private readonly IDbContextScope dbContextScope;

        public BoundaryCommands(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string filename, int maxBoundaries = int.MaxValue)
        {
            return SaveBoundariesInZip(nameColumn, x => x, filename, maxBoundaries);
        }

        public IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, Func<string, string> nameColumnParser, string filename, int maxBoundaries = int.MaxValue)
        {
            var boundaries = GetBoundariesFromZip(filename, nameColumn, nameColumnParser, maxBoundaries);
            SaveBoundaries(boundaries);
            return boundaries;
        }

        public void SaveBoundaries(IEnumerable<Boundary> boundaries)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                var repository = new BoundaryRepository(dbContext);
                repository.Save(boundaries);
                dbContext.SaveChanges();
            }
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

            var files = Directory.GetFiles(outputDirectory, "*.shp");

            var shp = files.FirstOrDefault();

            if (shp == null)
            {
                throw new Exception(String.Format("Unable to find a .shp file in the files extracted from {0}.", file));
            }

            return shp;
        }

        private IEnumerable<Boundary> GetBoundariesFromZip(string file, string nameColumn, Func<string, string> nameColumnParser, int maxBoundaries = int.MaxValue)
        {
            var shp = ExtractShapefileFromZip(file);
            return GetBoundariesFromShapefile(shp, nameColumn, nameColumnParser, maxBoundaries);
        }

        private static IEnumerable<Boundary> GetBoundariesFromShapefile(string file, string nameColumn, Func<string, string> nameColumnParser, int maxBoundaries = int.MaxValue)
        {
            if (!File.Exists(file))
            {
                throw new RuntimeException("The file '{0}' does not exist.", file);
            }

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
                    var geometry = (Geometry)featureSet.GetShape(i, true).ToGeometry();

                    yield return new Boundary
                    {
                        Name = nameColumnParser(featureSet.GetFeature(i).DataRow[nameColumn].ToString()),
                        Shape = DbGeography.FromText(writer.Write(geometry), 4326),
                    };
                }
            }
        }

    }
}
