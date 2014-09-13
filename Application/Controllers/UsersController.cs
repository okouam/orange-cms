using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.ViewModels;
using OrangeCMS.Domain;
using OrangeCMS.Domain.Services;
using Codeifier.OrangeCMS.Domain.Services.Parameters;

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
        public UserModel Create(CreateUserModel model)
        {
            var user = mappingEngine.Map<User>(model);
            userService.Save(user);
            return mappingEngine.Map<UserModel>(user);
        }

        [HttpGet, Route("users")]
        public IEnumerable<UserModel> All()
        {
            var users = userService.GetAll().Result;
            return mappingEngine.Map<IEnumerable<UserModel>>(users);
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            var user = userService.FindById(id);
            var userModel = mappingEngine.Map<UserModel>(user);
            return Ok(userModel);
        }

        [HttpPatch, Route("users/{id}")]
        public async Task<UserModel> Update(long id, UpdateUserModel model)
        {
            var user = await userService.Update(id, mappingEngine.DynamicMap<UpdateUserParams>(model));
            return mappingEngine.Map<UserModel>(user);
        }

        [HttpDelete, Route("users/{id}")]
        public IHttpActionResult Delete(long id)
        {
            userService.Delete(id);
            return Ok();
        }
    }
}
