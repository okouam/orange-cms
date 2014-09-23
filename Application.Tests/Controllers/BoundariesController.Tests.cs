using System.Linq;
using CodeKinden.OrangeCMS.Application.Controllers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Controllers
{
    [TestFixture]
    internal class BoundariesControllerTests : BaseTest
    {
        BoundariesController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            controller = container.GetInstance<BoundariesController>();
        }

        [Test]
        public async void When_getting_all_boundaries_retrieves_all_available()
        {
            var boundaryCount = GetDatabaseContext().Boundaries.Count();
            var boundaries = await controller.GetAll();
            Assert.That(boundaryCount, Is.EqualTo(boundaries.Count));
        }
    }
}
