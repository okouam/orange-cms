﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using OrangeCMS.Application.Providers;
using OrangeCMS.Application.Services;
using OrangeCMS.Application.ViewModels;

namespace OrangeCMS.Application.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly IMappingEngine mapper;
        private readonly ICategoryService categoryService;

        public CategoriesController(IIdentityProvider identityProvider, IMappingEngine mapper, ICategoryService categoryService)
            : base(identityProvider)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        [HttpGet, Route("categories")]
        public async Task<IList<CategoryModel>> GetAll()
        {
            var categories = await categoryService.FindByClient(CurrentClient.Id);
            var models = mapper.Map<IList<CategoryModel>>(categories);
            return models;
        }
    }
}
