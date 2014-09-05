using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Bounce.Framework;
using DbUp;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;
using Codeifier.OrangeCMS.Repositories;
using OrangeCMS.Application.Services;
using OrangeCMS.Domain;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class Tasks
    {
        [Task(Command = "db:create:development", Description = "Creates the development database.")]
        public void CreateDevelopmentDatabase(string userId, string password, string dataSource, string databaseName, string boundaries = "", string customers = "")
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
            
            GenerateData(sqlConnectionString, boundaryData, customerData);
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

        private async void GenerateData(string connectionString, string boundaryData, string customerData)
        {
            var dbContext = new DatabaseContext(connectionString);

            dbContext.Users.Add(new global::OrangeCMS.Domain.User
            {
                UserName = "test",
                Email = "tester@nowhere.com",
                Role = Roles.Administrator,
                Password = "Password$123"
            });

            dbContext.SaveChanges();

            var boundaryService = new BoundaryService();
            var boundaries = boundaryService.GetBoundariesFromZip(boundaryData, "name");
            Console.WriteLine("The boundaries have been read from their source file.");

            var boundaryRepository = new BoundaryRepository(dbContext);
            boundaryRepository.Save(boundaries);
            Console.WriteLine("The boundaries have been saved to the database.");

            var customerService = new CustomerService();
            await customerService.Import(customerData);
            Console.WriteLine("The customers have been saved to the database.");
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
