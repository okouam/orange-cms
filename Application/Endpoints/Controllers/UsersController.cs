using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.Endpoints.Attributes;
using CodeKinden.OrangeCMS.Application.Endpoints.ViewModels.Users;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services.Commands;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;
using CodeKinden.OrangeCMS.Domain.Services.Queries;

namespace CodeKinden.OrangeCMS.Application.Endpoints.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMappingEngine mappingEngine;
        private readonly IUserCommands userCommands;
        private readonly IUserQueries userQueries;

        public UsersController(IMappingEngine mappingEngine, IIdentityProvider identityProvider, IUserCommands userCommands, IUserQueries userQueries) : base(identityProvider)
        {
            this.mappingEngine = mappingEngine;
            this.userCommands = userCommands;
            this.userQueries = userQueries;
        }

        [HttpPost, Route("users"), Allowed(Role.Administrator, Role.System)]
        public UserModel Create(CreateUserModel model)
        {
            var user = mappingEngine.Map<User>(model);

            if (CurrentUser.IsAdministrator && (user.IsSystem || user.IsAdministrator))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            userCommands.Save(user);

            return mappingEngine.Map<UserModel>(user);
        }

        [HttpGet, Route("users"), Allowed(Role.Administrator, Role.System)]
        public IEnumerable<UserModel> All()
        {
            var users = userQueries.GetAll();
            return mappingEngine.Map<IEnumerable<UserModel>>(users);
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            var user = userQueries.FindById(id);
            var userModel = mappingEngine.Map<UserModel>(user);
            return Ok(userModel);
        }

        [HttpPatch, Route("users/{id}")]
        public async Task<UserModel> Update(long id, UpdateUserModel model)
        {
            if (CurrentUser.IsAdministrator)
            {
                var target = await userQueries.FindById(id);

                if ((target.IsAdministrator && target.Id != CurrentUser.Id) || target.IsSystem)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (CurrentUser.IsStandard)
            {
                if (CurrentUser.Id != id)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            var user = await userCommands.Update(id, mappingEngine.DynamicMap<UpdateUserParams>(model));
            return mappingEngine.Map<UserModel>(user);
        }

        [HttpDelete, Route("users/{id}"), Allowed(Role.Administrator, Role.System)]
        public IHttpActionResult Delete(long id)
        {
            var target = userQueries.FindById(id).Result;

            if (CurrentUser.IsAdministrator)
            {
                if (target.IsSystem || target.IsAdministrator)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            if (CurrentUser.Id == id)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            userCommands.Delete(id);

            return Ok();
        }
    }
}
