using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Enteties
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public required string? Email { get; set; } // email adress for authorization only

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string PasswordHash { get; set; }
        
        public virtual UserProfile? UserProfile { get; set; }
    }
}
