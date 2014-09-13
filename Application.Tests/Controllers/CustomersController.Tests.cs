using System.Linq;
using System.Net.Http;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;
using Codeifier.OrangeCMS.Repositories;

namespace OrangeCMS.Application.Tests.Controllers
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
        public async void When_getting_a_customer_retrieves_all_the_customer_details()
        {
            var customer = GetDatabaseContext().Customers.First();
            
            var model = await controller.Get(customer.Id);

            Assert.That(model.Id, Is.EqualTo(customer.Id));
            Assert.That(model.Longitude, Is.EqualTo(customer.Coordinates.Longitude));
            Assert.That(model.Latitude, Is.EqualTo(customer.Coordinates.Latitude));
            Assert.That(model.Telephone, Is.EqualTo(customer.Telephone));
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
        public void When_deleting_a_customers_removes_it_from_the_database()
        {
            var customer = GetDatabaseContext().Customers.First();

            var beforeCount = GetDatabaseContext().Customers.Count();

            controller.Delete(customer.Id);

            var afterCount = container.GetInstance<CustomerRepository>().CountAll();

            Assert.That(afterCount, Is.EqualTo(beforeCount - 1));
        }

        [Test]
        public async void When_importing_adds_the_customers_to_the_database()
        {
            var content = Helpers.CreateMultipartFormDataContent("Data\\Customers.csv");
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "import") { Content = content };
            var customers = await controller.Import();
        }
    }
}
