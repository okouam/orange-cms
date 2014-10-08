using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Spatial;

namespace CodeKinden.OrangeCMS.Domain.Models
{
    public class Boundary
    {
        public Boundary()
        {
            Customers = new Collection<Customer>();
        }

        public DbGeography Shape { get; set; }

        public string Name { get; set; }

        public long Id { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
