using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain;
using Codeifier.OrangeCMS.Domain.Models;
using Codeifier.OrangeCMS.Domain.Providers;
using CsvHelper;
using System.Data.Entity;
using MoreLinq;
using OrangeCMS.Domain.Services;
using Codeifier.OrangeCMS.Repositories;

namespace OrangeCMS.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbContextScope dbContextScope;

        public CustomerService(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public async Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                var query = dbContext.Customers;

                return await query.Take(numCustomers).ToListAsync();
            }
        }

        public IEnumerable<Customer> Search(string strMatch, int? boundary, int pageSize, int pageNum, bool withCoordinatesOnly)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
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

            using (var dbContext = dbContextScope.CreateDbContext())
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

        public string Export()
        {
            var filename = Path.GetTempFileName();

            using (var writer = File.CreateText(filename))
            {
                var csv = new CsvWriter(writer);
                csv.WriteHeader<Customer>();

                using (var dbContext = dbContextScope.CreateDbContext())
                {
                    var customers = dbContext.Customers.ToList();
                    foreach (var customer in customers)
                    {
                        csv.WriteRecord(customer);
                    }
                }
            }

            return filename;
        }

        public void Save(params Customer[] customers)
        {
            foreach (var batch in customers.Batch(1000))
            {
                using (var dbContext = dbContextScope.CreateDbContext())
                {
                    dbContext.Configuration.AutoDetectChangesEnabled = false;
                    new CustomerRepository(dbContext).Save(batch);
                    dbContext.SaveChanges();
                }
            }
        }

        private static DateTime? GetDate(CsvReader csv, string header)
        {
            var dateString = csv.GetField<String>(header);
            DateTime date;
            return DateTime.TryParse(dateString, out date) ? date : (DateTime?) null;
        }
    }
}
