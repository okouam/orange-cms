using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain;
using Codeifier.OrangeCMS.Domain.Models;
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
                Coordinates = Coordinates.Create(coordinates.Y, coordinates.X)
            };
        }

        public IEnumerable<Customer> Search(string strMatch, int? boundary, int pageSize, int pageNum, bool withCoordinatesOnly)
        {
            using (var dbContext = new DatabaseContext())
            {
                var query = dbContext.Customers.AsQueryable();

                if (!String.IsNullOrEmpty(strMatch))
                {
                    query = query.Where(x => x.Telephone.Contains(strMatch) || x.Name.Contains(strMatch) || x.Formula.Contains(strMatch));
                }

                if (withCoordinatesOnly)
                {
                    query = query.Where(x => x.Coordinates != null);
                }


                var customers = query.OrderBy(x => x.Name).Skip(pageNum).Take(pageSize).ToList();

                if (boundary.HasValue)
                {
                    var sql = String.Format("select telephone from customers where coordinates is not null AND EXISTS(SELECT * FROM Boundaries WHERE Boundaries.Id = {0} AND customers.Coordinates.STIntersects(Boundaries.Shape) = 0)", boundary.Value);
                    var inBoundary = dbContext.Database.SqlQuery<string>(sql).ToList();
                    return customers.Where(x => inBoundary.Contains(x.Telephone)).ToList();
                }

                return customers;
            }
        }

        public IEnumerable<Customer> Import(string filename)
        {
            var csv = new CsvReader(File.OpenText(filename));

            IList<Customer> customers;

            using (var dbContext = new DatabaseContext())
            {
                customers = dbContext.Customers.ToList();
            }

            while (csv.Read())
            {
                String key;
                var headers = csv.FieldHeaders;

                if (headers.Contains("Numéro de Téléphone")) key = csv.GetField<string>("Numéro de Téléphone").Trim();
                else if (headers.Contains("NUM_ADSL")) key = csv.GetField<string>("NUM_ADSL").Trim();
                else throw new Exception("A column called [NUM_ADSL] or [\"Numéro de Téléphone\"] must be provided.");

                var customer = customers.FirstOrDefault(x => x.Telephone == key) ?? new Customer {Telephone = key};

                double? longitude = null, latitude = null;
                if (headers.Contains("Coordonées GPS Longitude")) longitude = csv.GetField<double>("Coordonées GPS Longitude");
                if (headers.Contains("Coordonées GPS Latitude")) latitude = csv.GetField<double>("Coordonées GPS Latitude");
                if (longitude.HasValue && latitude.HasValue) customer.Coordinates = Coordinates.Create(longitude.Value, latitude.Value);

                if (headers.Contains("Photo de l'entrée")) customer.ImageUrl = csv.GetField<string>("Photo de l'entrée");

                if (headers.Contains("NOM")) customer.Name = csv.GetField<string>("NOM");
                if (headers.Contains("LOGIN")) customer.Login = csv.GetField<string>("LOGIN");
                if (headers.Contains("FORMULE")) customer.Formula = csv.GetField<string>("FORMULE");
                if (headers.Contains("DEBIT")) customer.Speed = csv.GetField<string>("DEBIT");
                if (headers.Contains("DATE_EXPIRATION"))
                {
                    customer.ExpiryDate = GetDate(csv, "DATE_EXPIRATION");
                    customer.NeverExpires = csv.GetField<string>("DATE_EXPIRATION") == "Illimite";
                }

                if (csv.FieldHeaders.Contains("ETAT")) customer.Status = csv.GetField<string>("ETAT");
            
                customers.Add(customer);
            }

            return customers;
        }

        private static DateTime? GetDate(CsvReader csv, string header)
        {
            var dateString = csv.GetField<String>(header);
            DateTime date;
            return DateTime.TryParse(dateString, out date) ? date : (DateTime?) null;
        }
    }
}
