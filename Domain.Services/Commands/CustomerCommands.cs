using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Commands.Importers;
using CodeKinden.OrangeCMS.Repositories;
using CsvHelper;
using MoreLinq;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands
{
    public class CustomerCommands : ICustomerCommands
    {
        private readonly IDbContextScope dbContextScope;

        public CustomerCommands(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
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
                var customer = Spreadsheet.ReadCustomerData(csv, customers);

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
    }
}
