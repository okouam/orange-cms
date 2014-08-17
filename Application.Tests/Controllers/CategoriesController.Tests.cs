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
            fakeSecurityService.AssignCurrentUser(GetSpecificUser(Roles.Standard));
            controller = container.GetInstance<CategoriesController>();
        }

        [Test]
        public void When_getting_all_boundaries_retrieves_all_available()
        {
            var categoryCount = GetDatabaseContext().Boundaries.Count();

            var categories = controller.GetAll();

            Assert.That(categoryCount, Is.EqualTo(categories.Count));
        }
    }
}
