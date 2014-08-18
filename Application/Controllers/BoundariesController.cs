using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Services;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class BoundariesController : BaseApiController
    {
        private readonly IMappingEngine mapper;
        private readonly IBoundaryService boundaryService;

        public BoundariesController(IMappingEngine mapper, IBoundaryService boundaryService, ISecurityService securityService) : base(securityService)
        {
            this.mapper = mapper;
            this.boundaryService = boundaryService;
        }

        [HttpGet, Route("boundaries")]
        public IList<BoundaryModel> GetAll()
        {
            var boundaries = boundaryService.FindByClient(CurrentClient.Id);
            var models = mapper.Map<IList<BoundaryModel>>(boundaries);
            return models;
        }
    }
}
