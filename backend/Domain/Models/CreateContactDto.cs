using Domain.Enteties;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CreateContactDto
    {
        [Required, EmailAddress]
        public string ContactEmail { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(500)]
        public string ContactDescription { get; set; }

        [Required]
        public ContactCategory Category { get; set; }
    }
}
