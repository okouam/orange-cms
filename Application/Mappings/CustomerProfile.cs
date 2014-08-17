using AutoMapper;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CreateCustomerModel, Customer>();

            CreateMap<CustomerModel, Customer>();
            CreateMap<Customer, CustomerModel>();

            CreateMap<Customer, CustomerSummaryModel>();

            CreateMap<UpdateCustomerModel, Customer>();
        }
    }
}
