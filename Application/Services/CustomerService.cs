using System;
using System.Collections.Generic;
using System.Linq;
using OrangeCMS.Domain;
using System.Data.Entity;

namespace OrangeCMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DatabaseContext dbContext;

        public CustomerService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Customer> FindByClient(long id)
        {
            return dbContext.Customers.Where(x => x.Client.Id == id);
        }

        public Customer Save(User user, Customer customer)
        {
            dbContext.Users.Attach(user);
            customer.CreatedBy = user;
            customer.Client = user.Client;
            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
            return customer;
        }

        public Customer FindById(long id)
        {
            return dbContext
                .Customers
                .Include(x => x.CreatedBy)
                .Include(x => x.Categories)
                .SingleOrDefault(x => x.Id == id);
        }

        public Customer Update(Customer newValues)
        {
            var customer = dbContext.Customers.Find(newValues.Id);
            var entry = dbContext.Entry(customer);
            entry.CurrentValues.SetValues(newValues);
            dbContext.SaveChanges();
            return customer;
        }

        public IEnumerable<Customer> Search(long client, string strMatch, long? category)
        {
            var query = dbContext.Customers.Where(x => x.Client.Id == client);

            if (!String.IsNullOrEmpty(strMatch))
            {
                query = query.Where(x => x.Name.Contains(strMatch) || x.Telephone.Contains(strMatch));
            }

            if (category.HasValue)
            {
                query = query.Where(x => x.Categories.Any(y => y.Id == category));
            }

            return query.OrderBy(x => x.Name).Take(100); // to review!
        }

        public void Delete(long id)
        {
            var customer = dbContext.Customers.Find(id);
            dbContext.Customers.Remove(customer);
            dbContext.SaveChanges();
        }
    }
}
