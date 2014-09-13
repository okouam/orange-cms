using AutoMapper;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class BoundaryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Boundary, BoundaryModel>();
        }
    }
}
