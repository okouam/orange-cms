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

        public void AssignCurrentUser(User current)
        {
            this.user = current;
        }
    }
}
