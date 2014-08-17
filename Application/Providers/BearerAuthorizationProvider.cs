using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace OrangeCMS.Application.Providers
{
    public class BearerAuthorizationProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var result = base.RequestToken(context);
            return result;
        }

        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            var result = base.ValidateIdentity(context);
            return result;
        }
    }
}
