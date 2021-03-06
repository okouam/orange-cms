﻿using AutoMapper;
using CodeKinden.OrangeCMS.Application.Endpoints.ViewModels;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class BoundaryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Boundary, BoundaryResource>()
                .ForMember(dest => dest.CustomerCount, src => src.MapFrom(x => x.Customers.Count));
        }
    }
}
