using System.Collections.Generic;

namespace OrangeCMS.Domain
{
    public class Location
    {
        public User CreatedBy { get; set; }

        public ICollection<Category> Categories { get; set; } 

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public long Id { get; set; }
    }
}
