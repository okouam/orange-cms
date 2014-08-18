using System;
using System.Data.SqlClient;
using DbUp;
using DbUp.Engine;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;
using OrangeCMS.Application;
using OrangeCMS.Application.Tests;

namespace OrangeCMS.Tooling
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

            var config = new Config();
            bool isSuccess;
            
            switch (option.Key.ToString())
            {
                case "D1":
                    DatabaseCleaner.Restore(config.SqlConnectionString, config.Seed.Name, config.Development);
                    isSuccess = true;
                    break;
                case "D2":
                    isSuccess = CreateBackupForTests(config).Successful;
                    break;
                default:
                    throw new Exception("Unknown option requested.");
            }
            
            Console.WriteLine("Press any key to continue...");

            Console.ReadLine();

            return isSuccess ? 0 : -1;
        }

        private static DatabaseUpgradeResult CreateBackupForTests(Config cfg)
        {
            RecreateDatabase(cfg);
            var result = RunDatabaseMigrations(cfg);
            GenerateData(cfg);
            DatabaseCleaner.Backup(cfg.SqlConnectionString, cfg.Seed.Name);
            return result;
        }

        private static void GenerateData(Config cfg)
        {
           new TestDataGenerator()
              .WithUsers(50)
              .WithCustomers(5000)
              .WithCategories(50)
              .WithBoundaries(30)
              .SetupDatabase(new TestDataContext(cfg.Seed.ConnectionString));
        }

        private static DatabaseUpgradeResult RunDatabaseMigrations(Config cfg)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(cfg.Seed.ConnectionString)
                .LogScriptOutput()
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly)
                .Build();

            return upgrader.PerformUpgrade();
        }

        private static void RecreateDatabase(Config cfg)
        {
            var connection = new ServerConnection(new SqlConnection(cfg.SqlConnectionString));

            var server = new Server(connection);

            if (server.Databases.Contains(cfg.Seed.Name))
            {
                try
                {
                    log.Info("Dropping the database {0}.", cfg.Seed.Name);
                    server.KillDatabase(cfg.Seed.Name);
                    server.Databases[cfg.Seed.Name].Drop();
                }
                catch
                {
                    log.Error("An error occured when attempting to drop the database {0}.", cfg.Seed.Name);
                }

            }

            log.Info("Creating the database {0}.", cfg.Seed.Name);
            var database = new Database(server, cfg.Seed.Name);
            database.Create();
        }
    }
}
