using AutoMapper;
using CodeKinden.OrangeCMS.Application.Endpoints.ViewModels.Users;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Application.Mappings
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserResource>();

            CreateMap<CreateUserMessage, User>()
                .ForMember(dest => dest.Id, src => src.Ignore());

            CreateMap<User, UserSummaryResource>();
        }
    }
}
