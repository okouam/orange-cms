using AutoMapper;
using Codeifier.OrangeCMS.Domain;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Mappings
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
