using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Repositories
{
    [TestFixture]
    class CustomerRepositoryTests : RepositoryTest
    {
        [Test]
        public void When_changing_a_customer_updates_the_related_boundary_mappings()
        {
            Boundary boundary = null;
            Customer customer = null;

            WithDbContext(dbContext =>
            {
                boundary = new Boundary { Name = "Test", Shape = DbGeography.PolygonFromText("POLYGON((-1 -1, -1 1, 1 1, 1 -1, -1 -1))", 4326) };
                new BoundaryRepository(dbContext).Save(boundary);
            });

            WithDbContext(dbContext =>
            {
                customer = new Customer { Telephone = "1234567890", Coordinates = Coordinates.Create(0, 0) };
                new CustomerRepository(dbContext).Save(customer);
            });
            
            WithDbContext(dbContext =>
            {
                Assert.That(new BoundaryRepository(dbContext).Get(boundary.Id).Customers.Count, Is.EqualTo(1));
                Assert.That(new CustomerRepository(dbContext).Get(customer.Id).Boundaries.Count, Is.EqualTo(1));
            });
            
            WithDbContext(dbContext => dbContext.Customers.Remove(dbContext.Customers.First()));

            WithDbContext(dbContext =>
            {
                Assert.That(dbContext.Boundaries.Find(boundary.Id).Customers.Count, Is.EqualTo(0));
            });
        }
    }
}
