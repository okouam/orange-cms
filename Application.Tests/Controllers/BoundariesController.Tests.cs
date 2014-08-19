using System.Linq;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests.Controllers
{
    [TestFixture]
    class BoundariesControllerTests : BaseTest
    {
        private BoundariesController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            FakeIdentityProvider.AssignCurrentUser(GetSpecificUser(Roles.Standard));
            controller = container.GetInstance<BoundariesController>();
        }

        [Test]
        public void When_getting_all_boundaries_retrieves_all_available()
        {
            var boundaryCount = GetDatabaseContext().Boundaries.Count();

            var boundaries = controller.GetAll();

            Assert.That(boundaryCount, Is.EqualTo(boundaries.Count));
        }
    }
}
