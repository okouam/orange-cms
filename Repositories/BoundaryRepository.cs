using System.Collections.Generic;
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
            foreach (var boundary in boundaries)
            {
                dbContext.Boundaries.Add(boundary);
            }

            return boundaries;
        }
    }
}
