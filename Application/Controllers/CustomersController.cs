using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Codeifier.OrangeCMS.Domain;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain.Services;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService customerService;
        private readonly IMappingEngine mappingEngine;

        public CustomersController(IIdentityProvider identityProvider, ICustomerService customerService, IMappingEngine mappingEngine)
            : base(identityProvider)
        {
            this.customerService = customerService;
            this.mappingEngine = mappingEngine;
        }

        [HttpPost, Route("customers/{id}")]
        public async Task<CustomerModel> Get(long id)
        {
            var customer = await customerService.FindById(id);
            var model = mappingEngine.Map<CustomerModel>(customer);
            return model;
        }

        [HttpDelete, Route("customers/{id}")]
        public void Delete(long id)
        {
            customerService.Delete(id);
        }

        [HttpGet, Route("customers/import")]
        public async Task<IEnumerable<Customer>> Import()
        {
            var results = new List<Customer>();

            if (Request.Content.IsMimeMultipartContent())
            {
                var destination = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(destination);
                var streamProvider = new MultipartFormDataStreamProvider(destination);
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (var file in streamProvider.FileData)
                {
                    var customers = await customerService.Import(new FileInfo(file.LocalFileName).FullName);
                    results.AddRange(customers);
                }   
            }

            return results;
        }

        [HttpGet, Route("customers")]
        public async Task<IList<CustomerModel>> Search(string strMatch = null, int pageSize = 100, int pageNum = 0)
        {
            var customers = await customerService.Search(strMatch, pageSize, pageNum);
            return mappingEngine.Map<IList<CustomerModel>>(customers);
        }
    }
}
