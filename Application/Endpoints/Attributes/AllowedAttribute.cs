using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Application.Endpoints.Attributes
{
    public class AllowedAttribute : ActionFilterAttribute
    {
        private readonly IIdentityProvider IdentityProvider = new IdentityProvider(new DbContextScope(ConfigurationProvider.ConnectionString));

        private readonly Role[] roles;

        public AllowedAttribute(params Role[] roles)
        {
            this.roles = roles;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var user = IdentityProvider.Identify(actionContext.RequestContext.Principal.Identity as ClaimsIdentity);

            if (roles.Any(role => user.Role == role.ToString()))
            {
                actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have the necessary access rights.");
            }
        }
    }
}