using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    [Authorize]
    public class BoundariesController : BaseApiController
    {
        private readonly IBoundaryQueries boundaryService;
        private readonly IMappingEngine mappingEngine;

        public BoundariesController(IBoundaryQueries boundaryService, IIdentityProvider identityProvider, IMappingEngine mappingEngine) : base(identityProvider)
        {
            this.boundaryService = boundaryService;
            this.mappingEngine = mappingEngine;
        }

        [HttpGet, Route("boundaries")]
        public IEnumerable<BoundaryModel> GetAll()
        {
            var boundaries = boundaryService.GetAll();
            return mappingEngine.Map<IEnumerable<BoundaryModel>>(boundaries.Where(x => x.Customers.Count > 0));
        }
    }
}
