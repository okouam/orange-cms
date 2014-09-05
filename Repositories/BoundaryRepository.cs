using System.Collections.Generic;
using System.Threading.Tasks;
using OrangeCMS.Domain;

namespace Codeifier.OrangeCMS.Repositories
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

            dbContext.SaveChanges();

            return boundaries;
        }
    }
}
