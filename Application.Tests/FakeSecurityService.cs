using OrangeCMS.Application.Services;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests
{
    class FakeSecurityService : SecurityService
    {
        private User currentUser;

        public FakeSecurityService(User currentUser, DatabaseContext dbContext) : base(dbContext)
        {
            this.currentUser = currentUser;
        }

        public override User CurrentUser
        {
            get { return currentUser; }
        }

        public Client CurrentClient
        {
            get { return currentUser.Client; }
        }

        public void AssignCurrentUser(User user)
        {
            currentUser = user;
        }
    }
}
