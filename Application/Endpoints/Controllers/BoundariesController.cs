using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.Endpoints.ViewModels;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services;

namespace CodeKinden.OrangeCMS.Application.Endpoints.Controllers
{
    [Authorize]
    public class BoundariesController : BaseApiController
    {
        private readonly IBoundaryQueries boundaryQueries;
        private readonly IMappingEngine mappingEngine;

        public BoundariesController(
            IBoundaryQueries boundaryQueries, 
            IIdentityProvider identityProvider, 
            IMappingEngine mappingEngine) : base(identityProvider)
        {
            this.boundaryQueries = boundaryQueries;
            this.mappingEngine = mappingEngine;
        }

        [HttpGet, Route("boundaries")]
        public IEnumerable<BoundaryResource> GetAll()
        {
            var boundaries = boundaryQueries.GetAll();
            return mappingEngine.Map<IEnumerable<BoundaryResource>>(boundaries.Where(x => x.Customers.Count > 0));
        }
    }
}
