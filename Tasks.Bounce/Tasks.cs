using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Bounce.Framework;
using Codeifier.OrangeCMS.Domain;
using DbUp;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using MoreLinq;
using NLog;
using Codeifier.OrangeCMS.Repositories;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.Services;
using OrangeCMS.Domain;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class Tasks
    {
        [Task(Command = "db:create:development", Description = "Creates the development database.")]
        public void CreateDevelopmentDatabase(string userId, string password, string dataSource, string databaseName, string boundaries = "", string customers = "", string customersOrange = "")
        {
            var sqlConnectionString = CreateOrReplaceDatabase(userId, password, dataSource, databaseName);
            
            RunDatabaseMigrations(sqlConnectionString);
            
            var boundaryData = String.IsNullOrEmpty(boundaries) ? Environment.CurrentDirectory + @"\Data\Countries.zip" : boundaries;

            Console.WriteLine("Using the boundary data contained in " + boundaryData);
            
            if (!File.Exists(boundaryData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", boundaryData));
            }

            var customerData = customers == "" ? Environment.CurrentDirectory + @"\Data\Customers.csv" : customers;

            if (!File.Exists(customerData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", customerData));
            }

            var customerOrangeData = customers == "" ? Environment.CurrentDirectory + @"\Data\Customers.FromOrange.csv" : customersOrange;

            if (!File.Exists(customerOrangeData))
            {
                throw new Exception(String.Format("The file '{0}' could not be found.", customerOrangeData));
            }


            GenerateData(sqlConnectionString, boundaryData, customerData, customerOrangeData);
        }

        [Task(Command = "db:create", Description = "Creates an empty database.")]
        public void CreateDatabase(string userId, string password, string dataSource, string databaseName)
        {
            var sqlConnectionString = CreateOrReplaceDatabase(userId, password, dataSource, databaseName);
            RunDatabaseMigrations(sqlConnectionString);
        }

        private static void RunDatabaseMigrations(string sqlConnectionString)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(sqlConnectionString)
                .LogScriptOutput()
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly)
                .Build();

            upgrader.PerformUpgrade();
        }

        private void GenerateData(string connectionString, string boundaryData, string customerData, string customerOrangeData)
        {
            var dbContext = new DatabaseContext(connectionString);
            var identityProvider = new IdentityProvider();

            dbContext.Users.Add(new global::OrangeCMS.Domain.User
            {
                UserName = "test",
                Email = "tester@nowhere.com",
                Role = Roles.Administrator,
                Password = identityProvider.CreateHash("Password$123")
            });

            dbContext.SaveChanges();

            var boundaryService = new BoundaryService();
            var boundaries = boundaryService.GetBoundariesFromZip(boundaryData, "name");
            Console.WriteLine("The boundaries have been read from their source file.");

            var boundaryRepository = new BoundaryRepository(dbContext);
            boundaryRepository.Save(boundaries);
            dbContext.SaveChanges();
            Console.WriteLine("The boundaries have been saved to the database.");

            dbContext.Database.ExecuteSqlCommand("DELETE FROM dbo.Boundaries WHERE Shape.STIsValid() = 0");

            SaveCustomerData(connectionString, customerData);
            SaveCustomerData(connectionString, customerOrangeData);
            Console.WriteLine("The customers have been saved to the database.");
        }

        static void SaveCustomerData(string connectionString, string customerData)
        {
            var customers = new CustomerService().Import(customerData);

            foreach (var batch in customers.Batch(1000))
            {
                using (var batchContext = new DatabaseContext(connectionString))
                {
                    var customerRepository = new CustomerRepository(batchContext);
                    customerRepository.Save(batch);
                    batchContext.SaveChanges();
                }
            }
        }

        private string CreateOrReplaceDatabase(string userId, string password, string dataSource, string databaseName)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "tempdb",
                UserID = userId,
                Password = password,
                DataSource = dataSource
            };

            var server = GetServer(sqlConnectionStringBuilder.ToString());

            var existingDatabase = server.Databases[databaseName];

            if (existingDatabase != null)
            {
                log.Debug("The database [{0}] already exists and will be dropped.", databaseName);
                existingDatabase.Parent.KillDatabase(databaseName);
                server.Refresh();

                try
                {
                    existingDatabase.Drop();
                }
                catch
                {
                    server.Refresh();
                }

                log.Debug("The database [{0}] has been dropped.", databaseName);
            }

            var database = new Database(server, databaseName);
            database.Create();

            log.Debug("The database [{0}] has been created.", databaseName);

            sqlConnectionStringBuilder.InitialCatalog = databaseName;

            return sqlConnectionStringBuilder.ToString();
        }

        private Server GetServer(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            var serverConnection = new ServerConnection(connection);
            return new Server(serverConnection);
        }

        private static readonly Logger log = LogManager.GetCurrentClassLogger();
    }
}
