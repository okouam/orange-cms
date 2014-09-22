using OrangeCMS.Application.Providers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests
{
    public class FakeIdentityProvider : IdentityProvider
    {
        private User user;

        public FakeIdentityProvider(User user) : base(null)
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
