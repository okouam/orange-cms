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
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService customerService;
        private readonly IMappingEngine mapper;

        public CustomersController(IIdentityProvider identityProvider, ICustomerService customerService, IMappingEngine mapper)
            : base(identityProvider)
        {
            this.customerService = customerService;
            this.mapper = mapper;
        }

        [HttpPost, Route("customers")]
        public CustomerModel Create(CreateCustomerModel model)
        {
            var customer = mapper.Map<Customer>(model);
            customerService.Save(CurrentUser, customer);
            return mapper.Map<CustomerModel>(customer);
        }

        [HttpPost, Route("customers/{id}")]
        public CustomerModel Get(long id)
        {
            var customer = customerService.FindById(id);
            return mapper.Map<CustomerModel>(customer);
        }

        [HttpPatch, Route("customers/{id}")]
        public CustomerModel Update(long id, UpdateCustomerModel model)
        {
            var newValues = mapper.Map<Customer>(model);
            newValues.Id = id;
            var customer = customerService.Update(newValues);
            return mapper.Map<CustomerModel>(customer);
        }

        [HttpDelete, Route("customers/{id}")]
        public void Delete(long id)
        {
            customerService.Delete(id);
        }

        [HttpGet, Route("customers")]
        public IList<CustomerModel> Search(string strMatch = null, long? category = null)
        {
            var customers = customerService.Search(CurrentClient.Id, strMatch, category);
            return mapper.Map<IList<CustomerModel>>(customers);
        }
    }
}
