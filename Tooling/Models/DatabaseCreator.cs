using DbUp;
using DbUp.Engine;
using OrangeCMS.Application;

namespace OrangeCMS.Tooling.Models
{
    public class DatabaseCreator
    {
        private readonly DatabaseProvider databaseProvider;

        public DatabaseCreator(DatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        public DatabaseUpgradeResult CreateSeedDatabase(string sqlConnectionString, string boundaryData)
        {
            databaseProvider.CreateOrReplaceDatabase(sqlConnectionString);
            var result = RunDatabaseMigrations(sqlConnectionString);
            GenerateData(sqlConnectionString, boundaryData);
            return result;
        }

        private static void GenerateData(string connectionString, string boundaryData)
        {
            new TestDataGenerator()
               .WithUsers(50)
               .WithCustomers(5000)
               .WithCategories(50)
               .WithBoundaries(boundaryData)
               .SetupDatabase(new TestDataContext(connectionString));
        }

        private static DatabaseUpgradeResult RunDatabaseMigrations(string sqlConnectionString)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(sqlConnectionString)
                .LogScriptOutput()
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly)
                .Build();

            return upgrader.PerformUpgrade();
        }
    }
}
