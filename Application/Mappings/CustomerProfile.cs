using AutoMapper;
using Codeifier.OrangeCMS.Domain;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Customer, CustomerModel>();

            CreateMap<Customer, CustomerSummaryModel>();
        }
    }
}
