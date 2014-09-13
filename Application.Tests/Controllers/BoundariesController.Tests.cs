using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;

namespace OrangeCMS.Application.Tests.Controllers
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

        [Test]
        public async void When_getting_a_boundary_returns_the_WKT()
        {
            var boundary = GetDatabaseContext().Boundaries.First();
            var result = await controller.Get(boundary.Id);
            Assert.That(boundary.Id, Is.EqualTo(result.Id));
            Assert.That(boundary.Name, Is.EqualTo(result.Name));
        }

        [Test]
        public async void When_creating_a_boundary_stores_it_in_the_database()
        {
            var content = Helpers.CreateMultipartFormDataContent("Data\\Countries.zip");
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "boundaries") { Content = content };
            var boundaries = await controller.Create("name");
            Assert.That(boundaries.Count, Is.EqualTo(177));
        }
    }
}
