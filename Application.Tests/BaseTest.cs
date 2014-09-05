using System.Configuration;
using System.Linq;
using Codeifier.OrangeCMS.Application;
using NUnit.Framework;
using OrangeCMS.Application.Providers;
using OrangeCMS.Domain;
using Codeifier.OrangeCMS.Repositories;
using StructureMap;

namespace OrangeCMS.Application.Tests
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
            return container.GetInstance<DatabaseContext>();
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
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }
    }
}
