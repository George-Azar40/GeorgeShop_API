using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using GeorgeShop.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
           
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> Index() 
        {
           var categories = await _categoryService.GetAllCategories();

            return Ok(new
            {
                data = categories,
                message = _localizer["Success"].Value 
            });
        }
    }
}
