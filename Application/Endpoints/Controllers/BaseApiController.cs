using System.Security.Claims;
using System.Web.Http;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    public class BaseApiController : ApiController
    {
        protected User CurrentUser => identityProvider.Identify(User.Identity as ClaimsIdentity);

        protected readonly IIdentityProvider identityProvider;

        protected BaseApiController(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }
    }
}
