using System;

namespace CodeKinden.OrangeCMS.Application.ViewModels.Customers
{
    public class CustomerModel 
    {
        public string Telephone { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public long Id { get; set; }

        public string ImageUrl { get; set; }

        public string Status { get; set; }

        public string Formula { get; set; }

        public bool? NeverExpires { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Speed { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
