using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Domain.Services.Queries
{
    class BoundaryQueries : IBoundaryQueries
    {
        private readonly IDbContextScope dbContextScope;

        public BoundaryQueries(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public IEnumerable<Boundary> GetAll()
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return dbContext.Boundaries.Include(x => x.Customers).OrderBy(x => x.Name).AsNoTracking().ToList();
            }
        }

        public async Task<Boundary> Get(long id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return await dbContext.Boundaries.FindAsync(id);
            }
        }
    }
}
