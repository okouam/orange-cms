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
using CodeKinden.OrangeCMS.Domain.Services.Commands;
using CodeKinden.OrangeCMS.Domain.Services.Queries;

namespace CodeKinden.OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CustomersController : BaseApiController
    {
        private readonly IMappingEngine mappingEngine;
        private readonly ICustomerQueries customerQueries;
        private readonly ICustomerCommands customerCommands;

        public CustomersController(IIdentityProvider identityProvider, IMappingEngine mappingEngine, ICustomerCommands customerCommands, ICustomerQueries customerQueries)
            : base(identityProvider)
        {
            this.mappingEngine = mappingEngine;
            this.customerCommands = customerCommands;
            this.customerQueries = customerQueries;
        }

        [HttpGet, Route("customers/export"), Allowed(Role.Administrator, Role.System)]
        public HttpResponseMessage Export(string access_token)
        {
            HttpResponseMessage result;

            var filename = customerCommands.Export();

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

        [HttpPost, Route("customers/import"), Allowed(Role.Administrator, Role.System)]
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
                    var customers = customerCommands.Import(new FileInfo(file.LocalFileName).FullName);
                    results.AddRange(customers);
                }

                customerCommands.Save(results.ToArray());
            }

            return mappingEngine.Map<IEnumerable<CustomerModel>>(results);
        }

        [HttpPost, Route("customers"), Allowed(Role.Administrator, Role.System)]
        public CustomerModel Create(CustomerModel model)
        {
            var customer = mappingEngine.Map<Customer>(model);

            customerCommands.Save(customer);

            return mappingEngine.Map<CustomerModel>(customer);
        }

        [HttpDelete, Route("customers/{id}"), Allowed(Role.Administrator, Role.System)]
        public void Delete(int id)
        {
            customerCommands.Delete(id);
        }
        
        [HttpGet, Route("customers/{id}")]
        public CustomerModel Get(int id)
        {
            return mappingEngine.Map<CustomerModel>(customerQueries.GetById(id));
        }

        [HttpGet, Route("customers")]
        public IList<CustomerModel> Search(string strMatch = null, int? boundary = null, int pageSize = 100, int pageNum = 0)
        {
            if (!boundary.HasValue && string.IsNullOrEmpty(strMatch)) return new List<CustomerModel>();
            var customers = customerQueries.Search(strMatch, boundary, int.MaxValue, pageNum, true);
            return mappingEngine.Map<IList<CustomerModel>>(customers);
        }
    }
}
