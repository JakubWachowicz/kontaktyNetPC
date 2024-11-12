using AutoMapper;
using Domain.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
    }
    //Service for category specific opereations
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //Get list of categories and their subcategories
        public List<CategoryDto> GetCategories()
        {
            var categories = _context.Categories;
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            foreach (var category in categories) { 
                CategoryDto categoryDto = new CategoryDto();
                categoryDto.SubcategoryName = new List<string>();
                if (category.Name != null) { 
                   categoryDto.Name = category.Name;

                }
                if (category.SubCategories != null) { 
                   categoryDto.SubcategoryName.AddRange(category.SubCategories);

                }
                categoryDtos.Add(categoryDto);  
            }
            return categoryDtos;
        }
    }
}
