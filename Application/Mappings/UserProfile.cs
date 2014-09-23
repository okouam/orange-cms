using AutoMapper;
using CodeKinden.OrangeCMS.Domain.Models;
using OrangeCMS.Application.ViewModels;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserModel>();

            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.Id, src => src.Ignore());

            CreateMap<User, UserSummaryModel>();
        }
    }
}
