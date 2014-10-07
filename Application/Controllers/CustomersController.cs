using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.ViewModels.Customers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService customerService;
        private readonly IMappingEngine mappingEngine;

        public CustomersController(IIdentityProvider identityProvider, IMappingEngine mappingEngine, ICustomerService customerService)
            : base(identityProvider)
        {
            this.customerService = customerService;
            this.mappingEngine = mappingEngine;
        }


        [HttpGet, Route("customers/export")]
        public HttpResponseMessage Export(string access_token)
        {
            HttpResponseMessage result;

            var filename = customerService.Export();

            if (!File.Exists(filename))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(filename, FileMode.Open, FileAccess.Read));
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {FileName = "Customers.csv"};
            }
            
            return result;
        }

        [HttpPost, Route("customers/import")]
        public async Task<IEnumerable<CustomerModel>> Import()
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
                    var customers = customerService.Import(new FileInfo(file.LocalFileName).FullName);
                    results.AddRange(customers);
                }

                customerService.Save(results.ToArray());
            }

            return mappingEngine.Map<IEnumerable<CustomerModel>>(results);
        }

        [HttpDelete, Route("customers/{id}")]
        public void Delete(int id)
        {
            customerService.Delete(id);
        }

        [HttpGet, Route("customers")]
        public IList<CustomerModel> Search(string strMatch = null, int? boundary = null, int pageSize = 100, int pageNum = 0)
        {
            if (!boundary.HasValue && String.IsNullOrEmpty(strMatch)) return new List<CustomerModel>();
            var customers = customerService.Search(strMatch, boundary, int.MaxValue, pageNum, true);
            return mappingEngine.Map<IList<CustomerModel>>(customers);
        }
    }
}
