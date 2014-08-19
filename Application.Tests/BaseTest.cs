using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GeoCMS.Application;
using NUnit.Framework;
using OrangeCMS.Application.Providers;
using OrangeCMS.Domain;
using StructureMap;

namespace OrangeCMS.Application.Tests
{
    class BaseTest
    {
        protected IContainer container;
        protected FakeIdentityProvider FakeIdentityProvider;

        protected User CurrentUser
        {
            get { return FakeIdentityProvider.User; }
        }

        protected Client CurrentClient
        {
            get { return FakeIdentityProvider.CurrentClient; }
        }

        protected DatabaseContext GetDatabaseContext()
        {
            return container.GetInstance<DatabaseContext>();
        }

        protected User GetSpecificUser(string role)
        {
            return GetDatabaseContext().Users.Where(x => x.Role == role).Include(x => x.Client).First();
        }

        [SetUp]
        public virtual void SetUp()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            var sourceDatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            DatabaseCleaner.Restore(connectionString, sourceDatabaseName, new SqlConnectionStringBuilder(connectionString).InitialCatalog);
            container = Startup.CreateContainer(x => x.For<IIdentityProvider>().Use<FakeIdentityProvider>().Singleton());
            FakeIdentityProvider = (FakeIdentityProvider)container.GetInstance<IIdentityProvider>();
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }
    }
}
