using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
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
