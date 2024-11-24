using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Controller for contact categories
    [Route("[controller]")]
    [ApiController, Authorize]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //Get all categories with their subcategories

        [HttpGet]
        public ActionResult<CategoryDto> Get()
        {
            var categoriesDto = _categoryService.GetCategories();
            return Ok(categoriesDto);
        }
    }
}
