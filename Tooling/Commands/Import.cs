using System;
using System.Data.SqlClient;
using NLog;

namespace OrangeCMS.Tooling.Commands
{
    class Import
    {
        static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Execute(string sourceConnectionString, string destinationConnectionString, params TableImportSpec[] tables)
        {
           
        }

    }
}
