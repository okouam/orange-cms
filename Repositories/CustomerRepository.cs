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

        public Customer Get(long id)
        {
            return dbContext.Customers.Where(x => x.Id == 1).Include(x => x.Boundaries).FirstOrDefault();
        }

        public void Save(params Customer[] customers)
        {
            foreach (var customer in customers)
            {
                dbContext.Customers.Attach(customer);
                dbContext.Entry(customer).State = customer.Id == 0 ? EntityState.Added : EntityState.Modified;
            }
        }

        public void Save(IEnumerable<Customer> customers)
        {
            Save(customers.ToArray());
        }
    }
}
