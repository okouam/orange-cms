using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels.Boundaries;
using CodeKinden.OrangeCMS.Domain.Models;
using OrangeCMS.Application.ViewModels;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class BoundaryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Boundary, BoundaryModel>();
        }
    }
}
