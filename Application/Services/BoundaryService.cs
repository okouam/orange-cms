using System.Collections.Generic;
using System.Linq;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class BoundaryService : IBoundaryService
    {
        private readonly DatabaseContext dbContext;

        public BoundaryService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Boundary> FindByClient(long id)
        {
            return dbContext.Boundaries.Where(x => x.Client.Id == id).OrderBy(x => x.Name);
        }
    }
}
