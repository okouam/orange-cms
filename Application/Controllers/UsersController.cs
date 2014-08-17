using System;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Services;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMappingEngine mappingEngine;

        public UsersController(IMappingEngine mappingEngine, ISecurityService securityService) : base(securityService)
        {
            this.mappingEngine = mappingEngine;
        }

        [HttpPost, Route("users")]
        public IHttpActionResult Create(CreateUserModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            var user = securityService.FindById(id);
            var userModel = mappingEngine.Map<UserModel>(user);
            return Ok(userModel);
        }

        [HttpPatch, Route("users/{id}")]
        public IHttpActionResult Update(long id, UpdateUserModel model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete, Route("users/{id}")]
        public IHttpActionResult Delete(long id)
        {
            securityService.Delete(id);
            return Ok();
        }
    }
}
