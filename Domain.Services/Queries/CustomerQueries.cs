using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Domain.Services.Queries
{
    public class CustomerQueries : ICustomerQueries
    {
        private readonly IDbContextScope dbContextScope;

        public CustomerQueries(IDbContextScope dbContextScope)
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

                if (!string.IsNullOrEmpty(strMatch))
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

                return boundary.HasValue ? customers.Where(x => x.Boundaries.Any(y => y.Id == boundary.Value)) : customers.ToList();
            }
        }

        public Customer GetById(int id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return dbContext.Customers.Find(id);
            }
        }
    }
}
