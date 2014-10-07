using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Repositories;

namespace CodeKinden.OrangeCMS.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext dbContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public long CountAll()
        {
            return dbContext.Customers.Count();
        }

        public void Save(params Customer[] customers)
        {
            foreach (var customer in customers)
            {
                if (customer.Id == 0)
                {
                    dbContext.Entry(customer).State = EntityState.Added;
                }
                else
                {
                    var existing = dbContext.Customers.Find(customer.Id);
                    dbContext.Entry(existing).CurrentValues.SetValues(customer);
                }

                dbContext.SaveChanges();
                var record = dbContext.Customers.Find(customer.Id);

                record.Boundaries.Clear();

                if (record.Coordinates != null)
                {
                    FindContainingBoundaries(record);
                }
            }
        }

        public void Save(IEnumerable<Customer> customers)
        {
            Save(customers.ToArray());
        }

        private void FindContainingBoundaries(Customer customer)
        {
            var boundaries = dbContext.Boundaries.Where(x => !x.Shape.Intersects(customer.Coordinates));

            if (boundaries.Any())
            {
                foreach (var boundary in boundaries)
                {
                    customer.Boundaries.Add(boundary);
                }
            }
        }
    }
}
