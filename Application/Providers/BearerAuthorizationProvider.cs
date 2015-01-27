using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace CodeKinden.OrangeCMS.Application.Providers
{
    public class BearerAuthorizationProvider : OAuthBearerAuthenticationProvider
    {
        // Required for when passing in the token via the URL and not the headers i.e. exporting
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
                return Task.FromResult<object>(null);
            }
            return base.RequestToken(context);
        }
    }
}
