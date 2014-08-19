using OrangeCMS.Application.Providers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests
{
    class FakeIdentityProvider : IdentityProvider
    {
        private User user;

        public FakeIdentityProvider(User user)
        {
            this.user = user;
        }

        public override User User
        {
            get { return user; }
        }

        public Client CurrentClient
        {
            get { return user.Client; }
        }

        public void AssignCurrentUser(User user)
        {
            this.user = user;
        }
    }
}
