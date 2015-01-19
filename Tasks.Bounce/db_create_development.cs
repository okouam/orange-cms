using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Services.Commands;
using CodeKinden.OrangeCMS.Fixtures;
using CodeKinden.OrangeCMS.Repositories;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    [TestFixture]
    public class db_create_development
    {
        [Test]
        public void Execute()
        {
            DB.CreateOrReplaceDatabase(ConfigurationProvider.ConnectionString);

            DB.RunDatabaseMigrations(ConfigurationProvider.ConnectionString, true);

            GenerateData(ConfigurationProvider.ConnectionString, int.MaxValue, int.MaxValue);
        }

        private static void GenerateData(string connectionString, int maxBoundaries, int maxCustomers)
        {
            var dbContextScope = new DbContextScope(connectionString);
           
            SaveBoundaryData(maxBoundaries, dbContextScope, "CodeKinden.OrangeCMS.Fixtures.Development.Quartiers.Abidjan.Commune.zip", "quartier_8");

            SaveBoundaryData(maxBoundaries, dbContextScope, "CodeKinden.OrangeCMS.Fixtures.Development.Quartiers_092010.zip", "Descriptio", x => Regex.Match(x, (@"<br/>Quartier : (.+)<br/>")).Groups[1].Value);

            FixInvalidBoundaries(dbContextScope);

            var customerData = FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Development.Customers.csv");

            if (!File.Exists(customerData))
            {
                throw new Exception(string.Format("The file '{0}' could not be found.", customerData));
            }

            var customerOrangeData = FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Development.Customers.FromOrange.csv");

            if (!File.Exists(customerOrangeData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", customerOrangeData));
            }
            
            SaveCustomerData(dbContextScope, customerData, maxCustomers);
            SaveCustomerData(dbContextScope, customerOrangeData, maxCustomers);
            Console.WriteLine("The customers have been saved to the database.");
        }

        private static void FixInvalidBoundaries(IDbContextScope dbContextScope)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Boundaries WHERE Shape.STIsValid() = 0");
            }
        }

        private static void SaveBoundaryData(int maxBoundaries, IDbContextScope dbContextScope, string resourceName, string columnName, Func<string, string> columnNameParser)
        {
            var boundaryData = FixtureManager.Extract(resourceName);

            if (!File.Exists(boundaryData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", boundaryData));
            }

            var boundaryCommands = new BoundaryCommands(dbContextScope);
            boundaryCommands.SaveBoundariesInZip(columnName, columnNameParser, boundaryData, maxBoundaries);
            Console.WriteLine("The boundaries from '{0}' have been saved to the database.", resourceName);
        }

        private static void SaveBoundaryData(int maxBoundaries, IDbContextScope dbContextScope, string resourceName, string columnName)
        {
            SaveBoundaryData(maxBoundaries, dbContextScope, resourceName, columnName, x => x);
        }

        private static void SaveCustomerData(IDbContextScope dbContextScope, string customerData, int maxCustomers)
        {
            var customerService = new CustomerCommands(dbContextScope);
            var customers = customerService.Import(customerData, maxCustomers);
            customerService.Save(customers.ToArray());
        }
    }
}