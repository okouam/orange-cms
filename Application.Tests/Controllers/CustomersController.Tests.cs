using System.Linq;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Application.Repositories;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests.CategoriesControllerTests
{
    [TestFixture]
    class CustomersControllerTests : BaseTest
    {
        private CustomersController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            fakeSecurityService.AssignCurrentUser(GetSpecificUser(Roles.Standard));
            controller = container.GetInstance<CustomersController>();
        }

        [Test]
        public void When_getting_a_customer_retrieves_all_the_customer_details()
        {
            var customer = GetDatabaseContext().Customers.First();
            
            var model = controller.Get(customer.Id);

            Assert.That(model.Id, Is.EqualTo(customer.Id));
            Assert.That(model.Name, Is.EqualTo(customer.Name));
            Assert.That(model.Longitude, Is.EqualTo(customer.Longitude));
            Assert.That(model.Latitude, Is.EqualTo(customer.Latitude));
            Assert.That(model.CreatedBy.Id, Is.EqualTo(customer.CreatedBy.Id));
            Assert.That(model.Telephone, Is.EqualTo(customer.Telephone));
        }

        [Test]
        public void When_searching_for_customers_can_search_on_name()
        {
            var customers = GetDatabaseContext().Customers.Where(x => x.Name.Contains("an"));

            var models = controller.Search("an");

            Assert.That(models.Count, Is.EqualTo(customers.Count()));
        }

        [Test]
        public void When_searching_for_customers_can_search_on_telephone()
        {
            var customers = GetDatabaseContext().Customers.Where(x => x.Telephone.Contains("12"));

            var models = controller.Search("12");

            Assert.That(models.Count, Is.EqualTo(customers.Count()));
        }

        [Test]
        public void When_searching_for_customers_can_search_by_category()
        {
            var category = GetDatabaseContext().Categories.First();

            var customers = GetDatabaseContext().Customers.Where(x => x.Categories.Any(y => y.Id == category.Id));

            var models = controller.Search(null, category.Id);

            Assert.That(models.Count, Is.EqualTo(customers.Count()));
        }

        [Test]
        public void When_creating_customers_populates_the_database()
        {
            var createCustomerModel = new CustomerModel
            {
                Name = "James Dean",
                Telephone = "+99-(0)5463-3408-0002",
                Longitude = 29,
                Latitude = 12
            };

            var beforeCount = GetDatabaseContext().Customers.Count();

            var model = controller.Create(createCustomerModel);
            Assert.That(model.Id, Is.Not.EqualTo(0));

            var afterCount = container.GetInstance<CustomerRepository>().CountAll(CurrentClient.Id);
            Assert.That(afterCount, Is.EqualTo(beforeCount + 1));
        }

        [Test]
        public void When_deleting_a_customers_removes_it_from_the_database()
        {
            var customer = GetDatabaseContext().Customers.First();

            var beforeCount = GetDatabaseContext().Customers.Count();

            controller.Delete(customer.Id);

            var afterCount = container.GetInstance<CustomerRepository>().CountAll(CurrentClient.Id);
            Assert.That(afterCount, Is.EqualTo(beforeCount - 1));
        }

        [Test]
        public void When_updating_customers_updates_the_database()
        {
            var customer = GetDatabaseContext().Customers.First();

            var updateCustomerModel = new UpdateCustomerModel
            {
                Name = "Roger Moore",
                Latitude = 13,
                Longitude = 13,
                Telephone = "+66-(0)2055-9386-0352"
            };

            var model = controller.Update(customer.Id, updateCustomerModel);
            Assert.That(model.Name, Is.EqualTo("Roger Moore"));

            model = controller.Get(customer.Id);
            Assert.That(model.Name, Is.EqualTo("Roger Moore"));
        }
    }
}
