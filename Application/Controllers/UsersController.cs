using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.Services;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMappingEngine mappingEngine;
        private readonly IUserService userService;

        public UsersController(IMappingEngine mappingEngine, IIdentityProvider identityProvider, IUserService userService) : base(identityProvider)
        {
            this.mappingEngine = mappingEngine;
            this.userService = userService;
        }

        [HttpPost, Route("users")]
        public IHttpActionResult Create(CreateUserModel model)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("users")]
        public IEnumerable<UserModel> All()
        {
            return mappingEngine.Map<IEnumerable<UserModel>>(userService.FindByClient(CurrentClient.Id));
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            var user = userService.FindById(id);
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
            userService.Delete(id);
            return Ok();
        }
    }
}
