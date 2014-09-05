using System;

namespace Codeifier.OrangeCMS.Domain
{
    public class Customer
    {
        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public string Telephone { get; set; }
    }
}
