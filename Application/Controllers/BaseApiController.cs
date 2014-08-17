using System.Web.Http;
using OrangeCMS.Application.Services;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Controllers
{
    public class BaseApiController : ApiController
    {
        protected Client CurrentClient 
        {
            get { return securityService.CurrentUser.Client; }
        }

        protected User CurrentUser
        {
            get { return securityService.CurrentUser; }
        }
        
        protected readonly ISecurityService securityService;

        protected BaseApiController(ISecurityService securityService)
        {
            this.securityService = securityService;
        }
    }
}
