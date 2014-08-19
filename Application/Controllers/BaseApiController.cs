using System.Web.Http;
using OrangeCMS.Application.Providers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Controllers
{
    public class BaseApiController : ApiController
    {
        protected Client CurrentClient 
        {
            get { return identityProvider.User.Client; }
        }

        protected User CurrentUser
        {
            get { return identityProvider.User; }
        }
        
        protected readonly IIdentityProvider identityProvider;

        protected BaseApiController(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }
    }
}
