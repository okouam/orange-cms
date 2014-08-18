using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;

namespace OrangeCMS.Application.Tests
{
    public class DatabaseCleaner
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Restore(string connectionString, string fromDatabaseName, string toDatabaseName)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true, 
                DataSource = connectionStringBuilder.DataSource
            };

            var connection = ConnectToServer(builder.ToString());

            log.Info("Restoring database '{0}'.", toDatabaseName);

            var restore = new Restore
            {
                Database = toDatabaseName, 
                Action = RestoreActionType.Database,
                ReplaceDatabase = true
            };

            var filename = GetBackupFileName(fromDatabaseName);
            log.Info("Using the backup database contained in '{0}.", filename);
            
            restore.Devices.AddDevice(filename, DeviceType.File);

            var server = new Server(connection);

            if (server.Databases.Contains(toDatabaseName))
            {
                try
                {
                    log.Info("Dropping the database {0}.", toDatabaseName);
                    server.KillDatabase(toDatabaseName);
                    server.Databases[toDatabaseName].Drop();
                }
                catch 
                {
                    log.Error("Failed to drop the database '{0}.", toDatabaseName);
                }

            }

            var tables = restore.ReadFileList(server);
            var rows = tables.Select();

            foreach (var row in rows)
            {
                var logicalName = row["LogicalName"].ToString();
                var newFilename = logicalName.EndsWith("log") ? Guid.NewGuid() + "_log.ldf" : Guid.NewGuid() + ".mdf";
                var newFileLocation = Path.Combine(Path.GetTempPath(), newFilename);
                restore.RelocateFiles.Add(new RelocateFile(logicalName, newFileLocation));
                log.Info("Moving '{0}' to '{1}'.", logicalName, newFileLocation);
            }

            restore.SqlRestore(server);

            log.Info("Restore completed.");
        }

        public static void Backup(string connectionString, string databaseName)
        {
            var connection = ConnectToServer(connectionString);

            var backup = new Backup
            {
                Action = BackupActionType.Database,
                Database = databaseName,
                Initialize = true
            };

            var filename = GetBackupFileName(databaseName);

            log.Info("Backing up the database '{0}' to the file '{1}'.", databaseName, filename);

            if (File.Exists(filename))
            {
                log.Info("Deleting the previous backup file '{0}'.", filename);
                File.Delete(filename);
            }

            backup.Devices.AddDevice(filename, DeviceType.File);
            backup.SqlBackup(new Server(connection));

            log.Info("The backup of '{0}' to the file '{1}' is complete.", databaseName, filename);
        }

        private static string GetBackupFileName(string databaseName)
        {
            var codebase = Assembly.GetExecutingAssembly().CodeBase;
            var outputDirectory = Path.GetDirectoryName(codebase).Replace("file:\\", string.Empty);
            return Path.Combine(GetSolutionDirectory(outputDirectory), databaseName + ".bak");
        }

        private static string GetSolutionDirectory(string outputDirectory)
        {
            return Path.GetFullPath(Path.Combine(outputDirectory, "..\\..\\..\\"));
        }

        private static ServerConnection ConnectToServer(string connectionString)
        {
            var sqlConnection = new SqlConnection(connectionString);
            return new ServerConnection(sqlConnection);
        }
    }
}
