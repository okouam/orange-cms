using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    [Authorize]
    public class BoundariesController : BaseApiController
    {
        private readonly IBoundaryService boundaryService;

        public BoundariesController(IBoundaryService boundaryService, IIdentityProvider identityProvider) : base(identityProvider)
        {
            this.boundaryService = boundaryService;
        }

        [HttpGet, Route("boundaries")]
        public async Task<IEnumerable<BoundaryWithCustomerCount>> GetAll()
        {
            return await boundaryService.GetAll();
        }
    }
}
