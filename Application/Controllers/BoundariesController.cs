using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IBoundaryService boundaryService;
        readonly IMappingEngine mappingEngine;

        public BoundariesController(IBoundaryService boundaryService, IIdentityProvider identityProvider, IMappingEngine mappingEngine) : base(identityProvider)
        {
            this.boundaryService = boundaryService;
            this.mappingEngine = mappingEngine;
        }

        [HttpGet, Route("boundaries")]
        public async Task<IEnumerable<BoundaryModel>> GetAll()
        {
            var boundaries = await boundaryService.GetAll();
            return mappingEngine.Map<IEnumerable<BoundaryModel>>(boundaries);
        }
    }
}
