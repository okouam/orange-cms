using OrangeCMS.Application.ViewModels.Clients;

namespace OrangeCMS.Application.ViewModels
{
    public class UserModel
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public ClientSummaryModel Client { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
