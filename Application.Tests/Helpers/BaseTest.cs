using System.Linq;
using Codeifier.OrangeCMS.Application;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Repositories;
using NUnit.Framework;
using StructureMap;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
{
    class BaseTest
    {
        protected IContainer container;
        protected FakeClock fakeClock;
        protected FakeIdentityProvider fakeIdentityProvider;

        protected User CurrentUser
        {
            get { return fakeIdentityProvider.User; }
        }

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
            container = Startup.CreateContainer(x =>
            {
                x.For<IIdentityProvider>().Use<FakeIdentityProvider>().Singleton();
                x.For<IClock>().Use<FakeClock>().Singleton();
            });
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
