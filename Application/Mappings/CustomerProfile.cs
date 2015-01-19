using System.Data.Entity.Spatial;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels.Customers;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Customer, CustomerModel>()
                .ForMember(dest => dest.Longitude, src => src.MapFrom(x => x.Coordinates.Longitude))
                .ForMember(dest => dest.Latitude, src => src.MapFrom(x => x.Coordinates.Latitude));

            CreateMap<CustomerModel, Customer>()
                .ForMember(dest => dest.Coordinates, src => src.MapFrom(x => CreateCoordinates(x)));
        }

        private static DbGeography CreateCoordinates(CustomerModel model)
        {
            var hasCoordinates = model.Longitude.HasValue && model.Latitude.HasValue;
            return hasCoordinates ? CreateDbGeography(model.Latitude, model.Longitude) : null;
        }

        private static DbGeography CreateDbGeography(double? latitude, double? longitude)
        {
            return DbGeography.PointFromText("POINT(" + latitude + " " + longitude + ")", 4326);
        }
    }
}
