using AutoMapper;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Application.ViewModels.Boundaries;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class BoundaryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<BoundaryModel, Boundary>();
            CreateMap<Boundary, BoundaryModel>();

            CreateMap<Boundary, BoundarySummaryModel>();
        }
    }
}
