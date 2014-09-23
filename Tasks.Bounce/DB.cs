using System;
using System.Data.SqlClient;
using CodeKinden.OrangeCMS.Repositories;
using DbUp;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    class DB
    {
        public static void RunDatabaseMigrations(string sqlConnectionString)
        {
            var upgrader = DeployChanges.To
                .SqlDatabase(sqlConnectionString)
                .LogScriptOutput()
                .WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly)
                .Build();

            upgrader.PerformUpgrade();
        }

        public static void CreateOrReplaceDatabase(string userId, string password, string dataSource, string databaseName)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "tempdb",
                UserID = userId,
                Password = password,
                DataSource = dataSource
            };

            var server = GetServer(sqlConnectionStringBuilder.ToString());

            Database existingDatabase;

            try
            {
                existingDatabase = server.Databases[databaseName];
            }
            catch (Exception e)
            {
                throw new Exception("Unable to connect using the connection string '" + sqlConnectionStringBuilder + "'", e);
            }

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
        }

        public static void CreateOrReplaceDatabase(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            CreateOrReplaceDatabase(builder.UserID, builder.Password, builder.DataSource, builder.InitialCatalog);
        }
        
        private static Server GetServer(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            var serverConnection = new ServerConnection(connection);
            return new Server(serverConnection);
        }
        
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
    }
}
