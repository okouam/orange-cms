using System.Collections.Generic;

namespace OrangeCMS.Domain
{
    public class Customer
    {
        public string Name { get; set; }

        public User CreatedBy { get; set; }

        public ICollection<Category> Categories { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public long Id { get; set; }

        public Client Client { get; set; }

        public string Telephone { get; set; }
    }
}
