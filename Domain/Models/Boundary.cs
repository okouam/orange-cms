using System.Data.Entity.Spatial;

namespace OrangeCMS.Domain
{
    public class Boundary
    {
        public DbGeography Shape { get; set; }

        public string Name { get; set; }

        public long Id { get; set; }
    }
}
