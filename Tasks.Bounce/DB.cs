using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CodeKinden.OrangeCMS.Repositories;
using Dapper;
using DbUp;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class DB
    {
        public static void RunDatabaseMigrations(string sqlConnectionString, bool logScriptOutput)
        {
            var upgrader = DeployChanges.To.SqlDatabase(sqlConnectionString);

            if (logScriptOutput) upgrader = upgrader.LogScriptOutput();

            var builder = upgrader.WithScriptsEmbeddedInAssembly(typeof(DatabaseContext).Assembly).Build();

            builder.PerformUpgrade();
        }

        public static void CreateOrReplaceDatabase(string connectionString)
        {
            var targetConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var tempDbConnectionStringBuilder = GetTempDbConnectionString(connectionString);

            var server = GetServer(tempDbConnectionStringBuilder.ToString());

            Database existingDatabase;
            var databaseName = targetConnectionStringBuilder.InitialCatalog;

            try
            {
                existingDatabase = server.Databases[databaseName];
            }
            catch (Exception e)
            {
                throw new Exception("Unable to connect using the connection string '" + tempDbConnectionStringBuilder + "'", e);
            }

            if (existingDatabase != null)
            {
                DropDatabaseSnapshots(server, existingDatabase);

                DropDatabase(databaseName, existingDatabase, server);
            }

            var database = new Database(server, databaseName);
            database.Create();

            log.Debug("The database [{0}] has been created.", databaseName);
        }

        public static string CreateSnapshot(string connectionString)
        {
            var targetConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var tempDbConnectionStringBuilder = GetTempDbConnectionString(connectionString);

            var server = GetServer(tempDbConnectionStringBuilder.ToString());

            var snapshotName = GetSnapshotName(targetConnectionStringBuilder.InitialCatalog, server.Databases);

            ValidateSnapshotParameters(server, targetConnectionStringBuilder.InitialCatalog, snapshotName);

            var database = server.Databases[targetConnectionStringBuilder.InitialCatalog];

            var snapshot = new Database(server, snapshotName) { DatabaseSnapshotBaseName = targetConnectionStringBuilder.InitialCatalog };
  
            foreach (FileGroup fg in database.FileGroups)
            {
                snapshot.FileGroups.Add(new FileGroup(snapshot, fg.Name));
            }

            foreach (FileGroup fg in database.FileGroups)
            {
                foreach (DataFile df in fg.Files)
                {
                    snapshot.FileGroups[fg.Name].Files.Add(new DataFile(snapshot.FileGroups[fg.Name], df.Name, Path.Combine(database.PrimaryFilePath, df.Name + ".ss")));
                }
            }

            snapshot.Create();

            return snapshotName;
        }

        public static void RollbackToSnapshot(string connectionString, string snapshotName)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;
            connectionStringBuilder.InitialCatalog = "Master";

            var server = GetServer(connectionStringBuilder.ConnectionString);
            server.KillAllProcesses(databaseName);

            using (var sqlConnection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                var cmd = string.Format("RESTORE DATABASE {0} FROM DATABASE_SNAPSHOT = '{1}';", databaseName, snapshotName);
                sqlConnection.Execute(cmd);
            }
        }

        private static void DropDatabase(string databaseName, Database existingDatabase, Server server)
        {
            log.Debug("The database [{0}] already exists and will be dropped.", databaseName);
            existingDatabase.Parent.KillDatabase(databaseName);
            server.Refresh();

            try
            {
                existingDatabase.Drop();
            }
            catch(Exception e)
            {
                log.Debug(e);
                server.Refresh();
            }

            log.Debug("The database [{0}] has been dropped.", databaseName);
        }

        private static string GetSnapshotName(string databaseName, IEnumerable existingDatabases)
        {
            var count = existingDatabases.Cast<Database>().Count(x => x.Name.Contains(databaseName + "_snapshot_"));
            return databaseName + "_snapshot_" + count + 1;
        }

        private static void DropDatabaseSnapshots(Server server, IDatabaseOptions existingDatabase)
        {
            var snapshots = server.Databases.Cast<Database>().Where(x => x.Name.StartsWith(GetSnapshotPrefix(existingDatabase))).ToList();

            foreach (var snapshot in snapshots)
            {
                DropSnapshot(server, snapshot);
            }
        }

        private static string GetSnapshotPrefix(IDatabaseOptions existingDatabase)
        {
            return existingDatabase.Name + "_snapshot_";
        }

        private static void DropSnapshot(IRefreshable server, Database snapshot)
        {
            log.Debug("The snapshot [{0}] exists and will be dropped.", snapshot.Name);

            try
            {
                snapshot.Drop();
            }
            catch
            {
                server.Refresh();
            }

            log.Debug("The snapshot [{0}] has been dropped.", snapshot.Name);
        }

        private static SqlConnectionStringBuilder GetTempDbConnectionString(string connectionString)
        {
            var tempDbConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                                                {
                                                    InitialCatalog = "tempdb"
                                                };
            return tempDbConnectionStringBuilder;
        }

        private static Server GetServer(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            var serverConnection = new ServerConnection(connection);
            return new Server(serverConnection);
        }

        private static void ValidateSnapshotParameters(Server server, string databaseToSnap, string snapshotName)
        {
            if (server.EngineEdition != Edition.EnterpriseOrDeveloper)
                throw new Exception("Snapshots are not supported in this edition of SQL Server. Enterprise or Developer edition is required.");

            var db = server.Databases[databaseToSnap];
            if (db == null)
                throw new Exception(string.Format("Specified Database does not exist: {0}", databaseToSnap));

            var snapshot = server.Databases[snapshotName];
            if (snapshot != null)
                throw new Exception(string.Format("A snapshot or database named {0} already exists", snapshotName));

            if (databaseToSnap == snapshotName)
                throw new Exception("Snapshot name must be different from Database name");
        }

        private static readonly Logger log = LogManager.GetCurrentClassLogger();
    }
}
