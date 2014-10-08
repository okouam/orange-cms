﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;
using CsvHelper;
using MoreLinq;

namespace CodeKinden.OrangeCMS.Domain.Services
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
                var query = dbContext.Customers.Include(x => x.Boundaries).AsNoTracking().AsQueryable();

                if (!String.IsNullOrEmpty(strMatch))
                {
                    query = query.Where(x => x.Telephone.Contains(strMatch) 
                        || x.Boundaries.Any(y => y.Name.Contains(strMatch))
                        || x.Name.Contains(strMatch) 
                        || x.Formula.Contains(strMatch));
                }

                if (withCoordinatesOnly)
                {
                    query = query.Where(x => x.Coordinates != null);
                }
                
                var customers = query.OrderBy(x => x.Name).Skip(pageNum).Take(pageSize).ToList();

                if (boundary.HasValue)
                {
                    return customers.Where(x => x.Boundaries.Any(y => y.Id == boundary.Value));
                }

                return customers.ToList();
            }
        }

        public IEnumerable<Customer> Import(string filename, int maxCustomers = int.MaxValue)
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

                if (customers.Count > maxCustomers) break;
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
                    var customers = dbContext.Customers.AsNoTracking().ToList();
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
            foreach (var batch in customers.Batch(500))
            {
                using (var dbContext = dbContextScope.CreateDbContext())
                {
                    dbContext.Configuration.AutoDetectChangesEnabled = false;
                    new CustomerRepository(dbContext).Save(batch);
                    dbContext.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
               dbContext.Customers.Remove(dbContext.Customers.Find(id));
               dbContext.SaveChanges();
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
