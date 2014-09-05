using AutoMapper;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
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
