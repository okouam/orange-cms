using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NLog;

namespace OrangeCMS.Tooling.Models
{
    public class DatabaseProvider
    {
        public bool RecreateDatabaseFromTemplate(string sourceConnectionString, string destinationConnectionString)
        {
            var sourceConnectionBuilder = new SqlConnectionStringBuilder(sourceConnectionString);
            var destinationConnectionBuilder = new SqlConnectionStringBuilder(destinationConnectionString);

            CreateOrReplaceDatabase(destinationConnectionBuilder);

            var sql = GenerateDatabaseCreationScript(sourceConnectionBuilder);

            ExecuteDatabaseCreationScript(destinationConnectionBuilder, sql);

            TransferData(sourceConnectionBuilder, destinationConnectionBuilder,
                new TableImportSpec("Contacts"),
                new TableImportSpec("Clients"),
                new TableImportSpec("Users"),
                new TableImportSpec("Boundaries"),
                new TableImportSpec("Customers"),
                new TableImportSpec("Categories"),
                new TableImportSpec("CustomerCategories"));

            return true;
        }

        public void CreateOrReplaceDatabase(SqlConnectionStringBuilder sqlConnectionString)
        {
            var databaseName = sqlConnectionString.InitialCatalog;

            var server = GetServer(sqlConnectionString.ToString());

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
        }

        public void CreateOrReplaceDatabase(string sqlConnectionString)
        {
            CreateOrReplaceDatabase(new SqlConnectionStringBuilder(sqlConnectionString));
        }

        private string GenerateDatabaseCreationScript(SqlConnectionStringBuilder sqlConnectionString)
        {
            var server = GetServer(sqlConnectionString.ToString());
            var databaseCreationScript = Commands.GenerateCreationScript.Execute(server.Databases[sqlConnectionString.InitialCatalog]);
            log.Debug("Executing the SQL contained in '{0}'", databaseCreationScript);
            return databaseCreationScript;
        }

        private void ExecuteDatabaseCreationScript(SqlConnectionStringBuilder sqlConnectionBuilder, string filename)
        {
            var server = GetServer(sqlConnectionBuilder.ToString());
            var ctx = server.ConnectionContext;
            ctx.ExecuteNonQuery(File.ReadAllText(filename));
            log.Debug("The database creation script " + filename + " has been executed in " + sqlConnectionBuilder.InitialCatalog + ".");
        }

        private void TransferData(SqlConnectionStringBuilder source, SqlConnectionStringBuilder destination, params TableImportSpec[] tables)
        {
            log.Debug("For the bulk importing, using source " + source);

            log.Debug("For the bulk importing, using destination: " + destination);

            using (var connection = new SqlConnection(source.ToString()))
            {
                connection.Open();

                foreach (var table in tables)
                {
                    log.Debug("Importing [{0}].", table);

                    var command = connection.CreateCommand();
                    command.CommandText = table.Query;
                    var reader = command.ExecuteReader();

                    var bulkCopy = new SqlBulkCopy(destination.ToString(), SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock)
                    {
                        BulkCopyTimeout = int.MaxValue,
                        DestinationTableName = String.Format("[dbo].[{0}]", table.TableName)
                    };

                    bulkCopy.WriteToServer(reader);

                    reader.Close();
                }
            }
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
