using System.Data.Entity.Spatial;

namespace CodeKinden.OrangeCMS.Domain.Models
{
    public class Boundary
    {
        public DbGeography Shape { get; set; }

        public string Name { get; set; }

        public long Id { get; set; }

        public long CustomerCount { get; set; }
    }
}
