using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels.Boundaries;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services;
using OrangeCMS.Application.ViewModels;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    [Authorize]
    public class BoundariesController : BaseApiController
    {
        private readonly IMappingEngine mapper;
        private readonly IBoundaryService boundaryService;

        public BoundariesController(IMappingEngine mapper, IBoundaryService boundaryService, IIdentityProvider identityProvider) : base(identityProvider)
        {
            this.mapper = mapper;
            this.boundaryService = boundaryService;
        }

        [HttpGet, Route("boundaries")]
        public async Task<IList<BoundaryModel>> GetAll()
        {
            var boundaries = await boundaryService.GetAll();
            var models = mapper.Map<IList<BoundaryModel>>(boundaries.OrderBy(x => x.Name));
            return models;
        }
    }
}
