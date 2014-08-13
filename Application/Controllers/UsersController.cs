using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrangeCMS.Application.Models;

namespace OrangeCMS.Application
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly AppContext appContext;

        public UsersController(AppContext appContext)
        {
            this.appContext = appContext;
        }

        [HttpPost, Route("users")]
        public HttpResponseMessage Create(CreateUserModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            return Ok(appContext.Users.Find(id));
        }

        [HttpPatch, Route("users/{id}")]
        public HttpResponseMessage Update(long id, UpdateUserModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete, Route("users/{id}")]
        public IHttpActionResult Delete(long id)
        {
            var user = appContext.Users.Find(id);
            appContext.Users.Remove(user);
            appContext.SaveChanges();
            return Ok();
        }

        [HttpGet, Route("users")]
        public HttpResponseMessage Search(SearchUsersModel model)
        {
            throw new NotImplementedException();
        }
    }
}
