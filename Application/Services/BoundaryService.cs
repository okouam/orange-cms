using System.Collections.Generic;
using System.Linq;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class BoundaryService : IBoundaryService
    {
        public IEnumerable<Boundary> FindByClient(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Boundaries.Where(x => x.Client.Id == id).OrderBy(x => x.Name).ToList();
            }
        }
    }
}
