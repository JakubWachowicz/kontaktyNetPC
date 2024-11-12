using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
        public List<string> SubcategoryName { get; set; }

    }
}
