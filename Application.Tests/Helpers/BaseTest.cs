using System.Configuration;
using System.Linq;
using Codeifier.OrangeCMS.Application;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Fakes;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Repositories;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using NUnit.Framework;
using StructureMap;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
{
    class BaseTest
    {
        protected IContainer container;
        protected FakeClock fakeClock;
        protected FakeIdentityProvider fakeIdentityProvider;

        protected User CurrentUser => fakeIdentityProvider.Identify(null);

        protected DatabaseContext GetDatabaseContext()
        {
            return container.GetInstance<IDbContextScope>().CreateDbContext();
        }

        protected User GetSpecificUser(string role)
        {
            return GetDatabaseContext().Users.First(x => x.Role == role);
        }

        [SetUp]
        public virtual void SetUp()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            DB.CreateOrReplaceDatabase(connectionString);
            DB.RunDatabaseMigrations(connectionString, false);

            container = Startup.CreateContainer(x =>
            {
                x.For<IIdentityProvider>().Use<FakeIdentityProvider>().Singleton();
                x.For<IClock>().Use<FakeClock>().Singleton();
            });

            var dbContext = new DatabaseContext(connectionString);

            dbContext.Users.Add(new User
            {
                UserName = "test",
                Email = "tester@nowhere.com",
                Role = Roles.Administrator,
                Password = new IdentityProvider(container.GetInstance<IDbContextScope>()).CreateHash("Password$123")
            });

            dbContext.SaveChanges();

            fakeIdentityProvider = (FakeIdentityProvider)container.GetInstance<IIdentityProvider>();
            fakeClock = (FakeClock)container.GetInstance<IClock>();
            fakeIdentityProvider.AssignCurrentUser(GetSpecificUser(Roles.Administrator));
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }
    }
}
