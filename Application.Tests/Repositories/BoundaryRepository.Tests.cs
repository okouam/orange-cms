using System.Data.Entity.Spatial;
using System.Linq;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Repositories
{
    [TestFixture]
    class BoundaryRepositoryTests : RepositoryTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void When_changing_a_boundary_updates_the_related_customer_mappings()
        {
            Boundary boundary = null;
            Customer customer = null;

            WithDbContext(dbContext =>
            {
                customer = new Customer {Telephone = "1234567890", Coordinates = Coordinates.Create(0, 0)};
                new CustomerRepository(dbContext).Save(customer);
            });
           
            WithDbContext(dbContext =>
            {
                boundary = new Boundary { Name = "Test", Shape = DbGeography.PolygonFromText("POLYGON((-1 -1, -1 1, 1 1, 1 -1, -1 -1))", 4326) };
                new BoundaryRepository(dbContext).Save(boundary);
            });

            WithDbContext(dbContext =>
            {
                Assert.That(new BoundaryRepository(dbContext).Get(boundary.Id).Customers.Count, Is.EqualTo(1));
                Assert.That(new CustomerRepository(dbContext).Get(customer.Id).Boundaries.Count, Is.EqualTo(1));
            });

            WithDbContext(dbContext => dbContext.Boundaries.Remove(dbContext.Boundaries.First()));
            
            WithDbContext(dbContext => {
                Assert.That(dbContext.Customers.Find(customer.Id).Boundaries.Count, Is.EqualTo(0));
            });
        }
    }
}
