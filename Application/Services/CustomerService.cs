using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotSpatial.Topology;
using OrangeCMS.Domain;
using System.Data.Entity;

namespace OrangeCMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        public async Task<IEnumerable<Customer>> FindByClient(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Customers.Where(x => x.Client.Id == id).ToListAsync();
            }
        }

        public async Task<Customer> Save(User user, Customer customer)
        {
            using (var dbContext = new DatabaseContext())
            {
                dbContext.Users.Attach(user);
                customer.CreatedBy = user;
                customer.Client = user.Client;
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
                return customer;
            }
        }

        public async Task<Customer> FindById(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext
                    .Customers
                    .Include(x => x.CreatedBy)
                    .Include(x => x.Categories)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Customer> Update(Customer newValues)
        {
            using (var dbContext = new DatabaseContext())
            {
                var customer = dbContext.Customers.Find(newValues.Id);
                var entry = dbContext.Entry(customer);
                entry.CurrentValues.SetValues(newValues);
                await dbContext.SaveChangesAsync();
                return customer;
            }
        }

        public async Task<IEnumerable<Customer>> Search(long client, string strMatch, long? category)
        {
            using (var dbContext = new DatabaseContext())
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

                return await query.OrderBy(x => x.Name).Take(100).ToListAsync();
            }
        }

        public async void Delete(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                var customer = dbContext.Customers.Find(id);
                dbContext.Customers.Remove(customer);
                await dbContext.SaveChangesAsync();
            }
        }

        public Customer CreateFakeCustomer(Client client, IList<Category> categories, IList<User> users, Coordinate coordinates)
        {
            return new Customer
            {
                Name = Faker.NameFaker.Name(),
                Telephone = Faker.PhoneFaker.InternationalPhone(),
                Longitude = (decimal) coordinates.X,
                Latitude = (decimal) coordinates.Y,
                Client = client,
                CreatedBy = users[Faker.NumberFaker.Number(0, users.Count)],
                Categories = categories.Sample(0, 3).ToList()
            };
        }
    }
}
