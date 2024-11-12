using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController,Authorize]
    public class CategoryController:ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<CategoryDto> Get()
        {
            var categoriesDto =_categoryService.GetCategories();
            return Ok(categoriesDto);
        }
    }
}
