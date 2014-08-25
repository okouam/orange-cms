using System;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using NLog;

namespace OrangeCMS.Tooling.Commands
{
    class GenerateCreationScript
    {
        static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static String Execute(Database database)
        {
            var filename = Path.GetTempFileName();
            log.Debug("The database creation script will be saved to '{0}'.", filename);

            var transfer = new Transfer(database)
            {
                CopyData = false,
                CopyAllObjects = true,
                PrefetchObjects = true,
                Options =
                {
                    Indexes = true,
                    WithDependencies = true,
                    DriAll = true,
                    ToFileOnly = true,
                    ScriptBatchTerminator = true,
                    ExtendedProperties = true,
                    IncludeHeaders = true,
                    NoFileGroup = true,
                    FileName = filename
                }
            };

            transfer.ScriptTransfer();
            log.Debug("The database creation script has been generated and saved to '{0}'.", filename);

            return filename;
        }
    }
}
