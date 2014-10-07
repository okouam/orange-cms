using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Bounce.Framework;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services;
using CodeKinden.OrangeCMS.Fixtures;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class db_create_development
    {
        [Task(Command = "db:create:development", Description = "Creates the development database.")]
        public void Execute(string targetConnectionString, int maxBoundaries = int.MaxValue, int maxCustomers = int.MaxValue)
        {
            //DB.CreateOrReplaceDatabase(targetConnectionString);

            //DB.RunDatabaseMigrations(targetConnectionString, true);

            GenerateData(targetConnectionString, maxBoundaries, maxCustomers);
        }

        private static void GenerateData(string connectionString, int maxBoundaries, int maxCustomers)
        {
            var dbContextScope = new DbContextScope(connectionString);

            var identityProvider = new IdentityProvider(dbContextScope);

            var dbContext = new DatabaseContext(connectionString);

            //dbContext.Users.Add(new User
            //{
            //    UserName = "test",
            //    Email = "tester@nowhere.com",
            //    Role = Roles.Administrator,
            //    Password = identityProvider.CreateHash("Password$123")
            //});

            //dbContext.SaveChanges();

            SaveBoundaryData(maxBoundaries, dbContextScope, "CodeKinden.OrangeCMS.Fixtures.Development.Quartiers.Abidjan.Commune.zip", "quartier_8");

            SaveBoundaryData(maxBoundaries, dbContextScope, "CodeKinden.OrangeCMS.Fixtures.Development.Quartiers_092010.zip", "Descriptio", x => Regex.Match(x, (@"<br/>Quartier : (.+)<br/>")).Groups[1].Value);

            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Boundaries WHERE Shape.STIsValid() = 0");

            //var customerData = FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Development.Customers.csv");

            //if (!File.Exists(customerData))
            //{
            //    throw new Exception(String.Format("The file '{0}' could not be found.", customerData));
            //}

            //var customerOrangeData = FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Development.Customers.FromOrange.csv");

            //if (!File.Exists(customerOrangeData))
            //{
            //    throw new Exception(String.Format("The file '{0}' could not be found.", customerOrangeData));
            //}



            //SaveCustomerData(dbContextScope, customerData, maxCustomers);
            //SaveCustomerData(dbContextScope, customerOrangeData, maxCustomers);
            Console.WriteLine("The customers have been saved to the database.");
        }

        private static void SaveBoundaryData(int maxBoundaries, IDbContextScope dbContextScope, string resourceName, string columnName, Func<string, string> columnNameParser)
        {
            var boundaryData = FixtureManager.Extract(resourceName);

            if (!File.Exists(boundaryData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", boundaryData));
            }

            var boundaryService = new BoundaryService(dbContextScope);
            boundaryService.SaveBoundariesInZip(columnName, columnNameParser, boundaryData, maxBoundaries);
            Console.WriteLine("The boundaries from '{0}' have been saved to the database.", resourceName);
        }

        private static void SaveBoundaryData(int maxBoundaries, IDbContextScope dbContextScope, string resourceName, string columnName)
        {
            SaveBoundaryData(maxBoundaries, dbContextScope, resourceName, columnName, x => x);
        }

        private static void SaveCustomerData(IDbContextScope dbContextScope, string customerData, int maxCustomers)
        {
            var customerService = new CustomerService(dbContextScope);
            var customers = customerService.Import(customerData, maxCustomers);
            customerService.Save(customers.ToArray());
        }
    }
}