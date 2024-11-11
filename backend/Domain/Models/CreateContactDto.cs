using Domain.Enteties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CreateContactDto
    {
        [EmailAddress, Required]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
