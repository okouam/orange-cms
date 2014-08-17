using AutoMapper;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();

            CreateMap<Category, CategorySummaryModel>();
        }
    }
}
