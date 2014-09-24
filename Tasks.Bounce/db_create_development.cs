using System;
using System.IO;
using System.Linq;
using Bounce.Framework;
using Codeifier.OrangeCMS.Domain.Providers;
using Codeifier.OrangeCMS.Repositories;
using Fixtures;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.Services;
using OrangeCMS.Domain;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class db_create_development
    {
        [Task(Command = "db:create:development", Description = "Creates the development database.")]
        public void Execute(string sourceConnectionString, string targetConnectionString, string customers = "", string customersOrange = "")
        {
            DB.CreateOrReplaceDatabase(targetConnectionString);

            DB.RunDatabaseMigrations(targetConnectionString);

            var customerData = customers == "" ? FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Customers.csv") : customers;

            if (!File.Exists(customerData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", customerData));
            }

            var customerOrangeData = customers == "" ? FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Customers.FromOrange.csv") : customersOrange;

            if (!File.Exists(customerOrangeData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", customerOrangeData));
            }

            GenerateData(targetConnectionString, customerData, customerOrangeData);
        }
        
        private static void GenerateData(string connectionString, string customerData, string customerOrangeData)
        {
            var dbContextScope = new DbContextScope(connectionString);

            var identityProvider = new IdentityProvider(dbContextScope);

            var dbContext = new DatabaseContext(connectionString);

            dbContext.Users.Add(new User
            {
                UserName = "test",
                Email = "tester@nowhere.com",
                Role = Roles.Administrator,
                Password = identityProvider.CreateHash("Password$123")
            });

            dbContext.SaveChanges();

            var boundaryService = new BoundaryService(dbContextScope);

            boundaryService.SaveBoundariesInZip("name", FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Countries.zip"));
            Console.WriteLine("The boundaries from '{0}' have been saved to the database.", "Countries.zip");

            //boundaryService.SaveBoundariesInZip("name", FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Quartiers.A.zip"));
            //Console.WriteLine("The boundaries from '{0}' have been saved to the database.", "Quartiers.A.zip");
            
            //boundaryService.SaveBoundariesInZip("name", FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Quartiers.B.zip"));
            //Console.WriteLine("The boundaries from '{0}' have been saved to the database.", "Quartiers.B.zip");

            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Boundaries WHERE Shape.STIsValid() = 0");

            SaveCustomerData(dbContextScope, customerData);
            SaveCustomerData(dbContextScope, customerOrangeData);
            Console.WriteLine("The customers have been saved to the database.");
        }
        
        private static void SaveCustomerData(IDbContextScope dbContextScope, string customerData)
        {
            var customerService = new CustomerService(dbContextScope);
            var customers = customerService.Import(customerData);
            customerService.Save(customers.ToArray());
        }
    }
}
