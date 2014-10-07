using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return Save(boundaries.ToArray());
        }

        public IEnumerable<Boundary> Save(params Boundary[] boundaries)
        {
            foreach (var boundary in boundaries)
            {
                var current = boundary;

                try
                {
                    var customers = dbContext.Customers
                        .Where(x => x.Coordinates != null)
                        .Where(x => !x.Coordinates.Intersects(current.Shape));

                    boundary.Customers.Clear();

                    if (customers.Any())
                    {
                        foreach (var customer in customers)
                        {
                            boundary.Customers.Add(customer);
                        }
                    }

                    dbContext.Boundaries.Add(boundary);
                }
                catch
                {
                    // This empty block is required to handle invalid geometries. 
                }
            }

            return boundaries;
        }
    }
}
