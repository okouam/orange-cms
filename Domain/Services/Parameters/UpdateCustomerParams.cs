using System.Collections.Generic;

namespace Codeifier.OrangeCMS.Domain.Services.Parameters
{
    public class UpdateCustomerParams
    {
        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public string Telephone { get; set; }

        public IList<long> Categories { get; set; }
    }
}
