using System.Collections.Generic;
using OrangeCMS.Application.Controllers;

namespace OrangeCMS.Application.ViewModels
{
    public class CustomerModel : CreateCustomerModel
    {
        public UserSummaryModel CreatedBy { get; set; }

        public ICollection<CategorySummaryModel> Categories { get; set; }

        public long Id { get; set; }
    }
}
