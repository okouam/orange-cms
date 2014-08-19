using AutoMapper;
using OrangeCMS.Application.ViewModels.Clients;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Mappings
{
    public class ClientProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Client, ClientSummaryModel>();
        }
    }
}
