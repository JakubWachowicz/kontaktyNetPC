using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LoginUserDto
    {
        [EmailAddress, Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
