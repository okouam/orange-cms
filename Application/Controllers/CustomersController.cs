
using System;
using System.Net.Http;
using System.Web.Http;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CustomersController : ApiController
    {
        [HttpPost, Route("customers")]
        public HttpResponseMessage Create(CreateCustomerModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("customers/{id}")]
        public HttpResponseMessage Get(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch, Route("customers/{id}")]
        public HttpResponseMessage Update(long id, UpdateCustomerModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete, Route("customers/{id}")]
        public HttpResponseMessage Delete(long id)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("customers")]
        public HttpResponseMessage Search(SearchCustomersModel model)
        {
            throw new NotImplementedException();
        }
    }
}
