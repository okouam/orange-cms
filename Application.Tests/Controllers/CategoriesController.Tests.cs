using System.Linq;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests.Controllers
{
    [TestFixture]
    class CategoriesControllerTests : BaseTest
    {
        private CategoriesController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            FakeIdentityProvider.AssignCurrentUser(GetSpecificUser(Roles.Standard));
            controller = container.GetInstance<CategoriesController>();
        }

        [Test]
        public async void When_getting_all_boundaries_retrieves_all_available()
        {
            var categoryCount = GetDatabaseContext().Categories.Count();
            var categories = await controller.GetAll();
            Assert.That(categoryCount, Is.EqualTo(categories.Count));
        }
    }
}
