using AutoMapper;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<UpdateUserModel, User>();

            CreateMap<UserModel, User>();
            CreateMap<User, UserModel>();

            CreateMap<CreateUserModel, User>();
            
            CreateMap<User, UserSummaryModel>();
        }
    }
}
