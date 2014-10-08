using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Spatial;

namespace CodeKinden.OrangeCMS.Domain.Models
{
    public class Customer
    {
        public Customer()
        {
            Boundaries = new Collection<Boundary>();
        }

        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string Telephone { get; set; }

        public string Status { get; set; }

        public string Formula { get; set; }

        public bool? NeverExpires { get; set; }

        public string Speed { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public DbGeography Coordinates { get; set; }

        public ICollection<Boundary> Boundaries { get; set; }
    }
}
