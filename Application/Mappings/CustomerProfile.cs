using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels.Customers;
using CodeKinden.OrangeCMS.Domain.Models;
using OrangeCMS.Application.ViewModels;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.Longitude, src => src.MapFrom(x => x.Coordinates.Longitude))
                .ForMember(dest => dest.Latitude, src => src.MapFrom(x => x.Coordinates.Latitude));
        }
    }
}
