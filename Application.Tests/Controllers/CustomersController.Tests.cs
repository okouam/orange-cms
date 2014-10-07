using System.Linq;
using System.Net.Http;
using CodeKinden.OrangeCMS.Application.Controllers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Fixtures;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Controllers
{
    [TestFixture]
    class CustomersControllerTests : BaseTest
    {
        private CustomersController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            controller = container.GetInstance<CustomersController>();
        }

        [Test]
        public void When_searching_for_customers_can_search_on_telephone()
        {
            var customers = GetDatabaseContext().Customers.Where(x => x.Telephone.Contains("12") && x.Coordinates != null);

            var models = controller.Search("12", null, int.MaxValue);

            Assert.That(models.Count, Is.EqualTo(customers.Count()));
        }

        [Test]
        public void When_searching_for_customers_can_search_on_boundary()
        {
            var customers = GetDatabaseContext().Customers.Where(x => x.Telephone.Contains("12") && x.Coordinates != null);
            var models = controller.Search("12", 3, int.MaxValue);
            Assert.That(models.Count, Is.EqualTo(customers.Count()));
        }

        [Test]
        public async void When_importing_adds_the_customers_to_the_database()
        {
            var fixture = FixtureManager.Extract("CodeKinden.OrangeCMS.Fixtures.Development.Customers.csv");
            var content = FakeFormData.CreateMultipartFormDataContent(fixture);
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "import") { Content = content };
            var customers = await controller.Import();
            Assert.That(customers.Count(), Is.EqualTo(339));
        }
    }
}
