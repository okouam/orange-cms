using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Services;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly IMappingEngine mapper;
        private readonly ICategoryService categoryService;

        public CategoriesController(ISecurityService securityService, IMappingEngine mapper, ICategoryService categoryService) : base(securityService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        [HttpGet, Route("categories")]
        public IList<CategoryModel> GetAll()
        {
            var categories = categoryService.FindByClient(CurrentClient.Id);
            var models = mapper.Map<IList<CategoryModel>>(categories);
            return models;
        }
    }
}
