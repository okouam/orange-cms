using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Repositories;

namespace CodeKinden.OrangeCMS.Repositories
{
    public class BoundaryRepository : IBoundaryRepository
    {
        private readonly DatabaseContext dbContext;

        public BoundaryRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Boundary> Save(IEnumerable<Boundary> boundaries)
        {
            if (boundaries == null || !boundaries.Any()) return Enumerable.Empty<Boundary>();
            return Save(boundaries.ToArray());
        }

        public Boundary Get(long id)
        {
            return dbContext.Boundaries.Where(x => x.Id == id).Include(x => x.Customers).FirstOrDefault();
        }

        public IEnumerable<Boundary> Save(params Boundary[] boundaries)
        {
            foreach (var boundary in boundaries)
            {
                dbContext.Boundaries.Add(boundary);
            }

            return boundaries;
        }
    }
}
