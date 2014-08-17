using System.Collections.Generic;

namespace OrangeCMS.Domain
{
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Client Client { get; set; }

        public ICollection<Customer> Customers { get; set; } 
    }
}
