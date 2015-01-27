using System;
using System.Security.Claims;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Fakes
{
    public class FakeIdentityProvider : IIdentityProvider
    {
        private User user;

        public void AssignCurrentUser(User current)
        {
            user = current;
        }

        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePassword(string password, string correctHash)
        {
            throw new NotImplementedException();
        }

        public string CreateHash(string password)
        {
            throw new NotImplementedException();
        }

        public User Identify(ClaimsIdentity identity)
        {
            return user;
        }
    }
}
