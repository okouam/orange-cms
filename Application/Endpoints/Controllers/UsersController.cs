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
        public UserResource Create(CreateUserMessage message)
        {
            var user = mappingEngine.Map<User>(message);

            if (CurrentUser.IsAdministrator && (user.IsSystem || user.IsAdministrator))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            userCommands.Save(user);

            return mappingEngine.Map<UserResource>(user);
        }

        [HttpGet, Route("users"), Allowed(Role.Administrator, Role.System)]
        public IEnumerable<UserResource> All()
        {
            var users = userQueries.GetAll();
            return mappingEngine.Map<IEnumerable<UserResource>>(users);
        }

        [HttpPost, Route("users/{id}")]
        public IHttpActionResult Get(long id)
        {
            var user = userQueries.FindById(id);
            var resource = mappingEngine.Map<UserResource>(user);
            return Ok(resource);
        }

        [HttpPatch, Route("users/{id}")]
        public async Task<UserResource> Update(long id, UpdateUserMessage message)
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

            var user = await userCommands.Update(id, mappingEngine.DynamicMap<UpdateUserParams>(message));
            return mappingEngine.Map<UserResource>(user);
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
