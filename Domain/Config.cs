using System.Configuration;
using System.Data.SqlClient;

namespace OrangeCMS.Tooling
{
    public class Config
    {
        public string SqlConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Main"].ConnectionString; }
        }

        public DatabaseInfo Seed
        {
            get
            {
                var name = ConfigurationManager.AppSettings["Seed.Database"];
                return new DatabaseInfo(name, SqlConnectionString);
            }
        }

        public string Development
        {
            get { return ConfigurationManager.AppSettings["Development.Database"]; }
        }

        public string Production
        {
            get { return ConfigurationManager.AppSettings["Production.Database"]; }
        }

        public string Test
        {
            get { return ConfigurationManager.AppSettings["Test.Database"]; }
        }
    }

    public class DatabaseInfo
    {
        public DatabaseInfo(string name, string sqlConnectionString)
        {
            Name = name;
            var connectionStringBuilder = new SqlConnectionStringBuilder(sqlConnectionString) {InitialCatalog = name};
            ConnectionString = connectionStringBuilder.ToString();
        }

        public string Name { get; set; }

        public string ConnectionString { get; set; }
    }
}
