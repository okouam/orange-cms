using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GeoCMS.Application;
using NUnit.Framework;
using OrangeCMS.Application.Services;
using OrangeCMS.Domain;
using StructureMap;

namespace OrangeCMS.Application.Tests
{
    class BaseTest
    {
        protected IContainer container;
        protected FakeSecurityService fakeSecurityService;

        protected User CurrentUser
        {
            get { return fakeSecurityService.CurrentUser; }
        }

        protected Client CurrentClient
        {
            get { return fakeSecurityService.CurrentClient; }
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
            container = Startup.CreateContainer(x => x.For<ISecurityService>().Use<FakeSecurityService>().Singleton());
            fakeSecurityService = (FakeSecurityService) container.GetInstance<ISecurityService>();
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }
    }
}
