using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain;
using CsvHelper;
using DotSpatial.Topology;
using OrangeCMS.Domain;
using System.Data.Entity;
using OrangeCMS.Domain.Services;
using Codeifier.OrangeCMS.Domain.Services.Parameters;
using Codeifier.OrangeCMS.Repositories;

namespace OrangeCMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        public async Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue)
        {
            using (var dbContext = new DatabaseContext())
            {
                var query = dbContext.Customers;

                return await query.Take(numCustomers).ToListAsync();
            }
        }

        public async Task<Customer> Save(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException("customer", "No customer was provided when saving a customer.");

            using (var dbContext = new DatabaseContext())
            {
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
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Customer> Update(long id, UpdateCustomerParams newValues)
        {
            if (newValues == null) throw new ArgumentNullException("newValues", "No service parameters were provided when updating a customer.");

            using (var dbContext = new DatabaseContext())
            {
                var customer = dbContext.Customers.Find(id);
                var entry = dbContext.Entry(customer);
                entry.CurrentValues.SetValues(newValues);    
                await dbContext.SaveChangesAsync();
                return customer;
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

        public Customer CreateFakeCustomer(IList<User> users, Coordinate coordinates)
        {
            return new Customer
            {
                Telephone = Faker.PhoneFaker.InternationalPhone(),
                Longitude = (decimal) coordinates.X,
                Latitude = (decimal) coordinates.Y
            };
        }

        public async Task<IEnumerable<Customer>> Search(string strMatch, int pageSize, int pageNum)
        {
            using (var dbContext = new DatabaseContext())
            {
                var query = dbContext.Customers.AsQueryable();

                if (!String.IsNullOrEmpty(strMatch))
                {
                    query = query.Where(x => x.Telephone.Contains(strMatch));
                }

                return await query.OrderBy(x => x.Telephone).Skip(pageNum).Take(pageSize).ToListAsync();
            }
        }

        public async Task<IEnumerable<Customer>> Import(string filename)
        {
            var csv = new CsvReader(File.OpenText(filename));
            var customers = new List<Customer>();

            using (var dbContext = new DatabaseContext())
            {
                while (csv.Read())
                {
                    throw new Exception(csv.GetField<string>("Date Created"));

                    var customer = new Customer
                    {
                        Longitude = csv.GetField<decimal>("Coordonées GPS Longitude"),
                        Latitude = csv.GetField<decimal>("Coordonées GPS Latitude"),
                        Telephone = csv.GetField<string>("Numéro de Téléphone"),
                        ImageUrl = csv.GetField<string>("Photo de l'entrée"),
                        DateCreated = csv.GetField<DateTime>("Date Created")
                    };
                    dbContext.Customers.Add(customer);
                    customers.Add(customer);
                }
  
                await dbContext.SaveChangesAsync();
            }

            return customers;
        }
    }
}
