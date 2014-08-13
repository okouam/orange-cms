using System;
using System.Net.Http;
using System.Web.Http;
using OrangeCMS.Application.Controllers;

namespace OrangeCMS.Application
{
    [Authorize]
    public class CategoriesController : ApiController
    {
        [HttpPost, Route("categories/{id}")]
        public HttpResponseMessage Get(long id)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("categories")]
        public HttpResponseMessage Search(SearchCategoriesModel model)
        {
            throw new NotImplementedException();
        }
    }
}
