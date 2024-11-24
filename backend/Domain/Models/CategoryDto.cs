using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
        public List<string> SubcategoryName { get; set; }

    }
}
