using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using OrangeCMS.Application.Services;

namespace OrangeCMS.Application.Providers
{
    public class TokenProvider : OAuthAuthorizationServerProvider
    {
        private readonly IIdentityProvider identityProvider;

        public TokenProvider(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }

#pragma warning disable 1998
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
#pragma warning restore 1998
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

#pragma warning disable 1998
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
#pragma warning restore 1998
        {
            var user = identityProvider.Authenticate(context.UserName, context.Password);
            
            if (user != null)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("id", user.Id.ToString()));
                var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                context.Validated(ticket);
            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
            }
            
            return base.GrantResourceOwnerCredentials(context);
        }
    }
}
