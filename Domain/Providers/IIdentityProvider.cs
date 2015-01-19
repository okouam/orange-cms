using System.Security.Claims;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Providers
{
    public interface IIdentityProvider
    {
        User Authenticate(string username, string password);

        bool ValidatePassword(string password, string correctHash);

        string CreateHash(string password);

        User Identify(ClaimsIdentity identity);
    }
}