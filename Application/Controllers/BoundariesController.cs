using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;
using OrangeCMS.Domain.Services;

namespace OrangeCMS.Application.Controllers
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

        [HttpGet, Route("boundaries/{id}")]
        public async Task<BoundaryModel> Get(long id)
        {
            var boundary = await boundaryService.Get(id);
            return mapper.Map<BoundaryModel>(boundary);
        }

        [HttpPost, Route("boundaries")]
        public async Task<IList<BoundaryModel>> Create(string nameColumn)
        {
            var result = new List<Boundary>();

            if (Request.Content.IsMimeMultipartContent())
            {
                var destination = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(destination);
                var streamProvider = new MultipartFormDataStreamProvider(destination);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (var file in streamProvider.FileData)
                {
                    var boundaries = boundaryService.SaveBoundariesInZip(nameColumn, new FileInfo(file.LocalFileName).FullName);
                    result.AddRange(boundaries);
                }   
            }

            return mapper.Map<IList<BoundaryModel>>(result);
        }
    }
}
