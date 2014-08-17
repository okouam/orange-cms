using System;
using System.Configuration;
using System.Data.SqlClient;
using DbUp;
using DbUp.Engine;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;
using OrangeCMS.Application;
using OrangeCMS.Application.Tests;
using OrangeCMS.Tooling;

namespace Tooling
{
    class Program
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            Console.WriteLine("Welcome to the OrangeCMS Tooling Pack\n");
            Console.WriteLine("Please select one of the options below: \n");
            Console.WriteLine("> 1. Restore seed database data to the development database.");
            Console.WriteLine("> 2. Recreate the seed database and create a backup for tests.");
            
            var option = Console.ReadKey();
            Console.WriteLine("\n");
            
            bool isSuccess;
            
            switch (option.Key.ToString())
            {
                case "D1":
                    isSuccess = CopyDataToDevelopmentDatabase();
                    break;
                case "D2":
                    isSuccess = CreateBackupForTests().Successful;
                    break;
                default:
                    throw new Exception("Unknown option requested.");
            }
            
            Console.WriteLine("Press any key to continue...");

            Console.ReadLine();

            return isSuccess ? 0 : -1;
        }

        static bool CopyDataToDevelopmentDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            var appSettings = ConfigurationManager.AppSettings;
            DatabaseCleaner.Restore(connectionString, appSettings["Seed"], appSettings["Development"]);
            return true;
        }

        static DatabaseUpgradeResult CreateBackupForTests()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["Seed"];

            RecreateDatabase(connectionString, databaseName);

            var result = RunDatabaseMigrations(connectionString, databaseName);

            GenerateData(connectionString, databaseName);

            DatabaseCleaner.Backup(connectionString, databaseName);
            return result;
        }

        private static void GenerateData(string connectionString, string databaseName)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };

           new TestDataGenerator()
              .WithUsers(50)
              .WithCustomers(5000)
              .WithCategories(50)
              .WithBoundaries(30)
              .SetupDatabase(new TestDataContext(connectionStringBuilder.ToString()));
        }

        private static DatabaseUpgradeResult RunDatabaseMigrations(string connectionString, string databaseName)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = databaseName
            };

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionStringBuilder.ToString())
                .LogScriptOutput()
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly)
                .Build();

            return upgrader.PerformUpgrade();
        }

        private static void RecreateDatabase(string connectionString, string databaseName)
        {
            var connection = ConnectToServer(connectionString);
            var server = new Server(connection);
            if (server.Databases.Contains(databaseName))
            {
                try
                {
                    log.Info("Dropping the database {0}.", databaseName);
                    server.KillDatabase(databaseName);
                    server.Databases[databaseName].Drop();
                }
                catch
                {
                    log.Error("An error occured when attempting to drop the database {0}.", databaseName);
                }

            }

            log.Info("Creating the database {0}.", databaseName);
            var database = new Database(server, databaseName);
            database.Create();
        }

        private static ServerConnection ConnectToServer(string connectionString)
        {
            var sqlConnection = new SqlConnection(connectionString);
            return new ServerConnection(sqlConnection);
        }
    }
}
