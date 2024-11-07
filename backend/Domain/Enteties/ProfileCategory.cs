using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enteties
{
    public enum CategoryNames
    {
        Buisness, Private, Other
    }
    public class ProfileCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public CategoryNames CategoryName { get; set; }

        public virtual List<ProfileSubCategory> ProfileSubCategories { get; set; }
    }
}
