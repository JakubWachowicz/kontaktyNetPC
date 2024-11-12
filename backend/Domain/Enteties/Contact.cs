using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enteties
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Association with the owner of the contact (UserProfile)
        //[ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }
        public required virtual UserProfile UserProfile { get; set; }
        public string? PhoneNumber { get; set; }
        [Required,EmailAddress]
        public string? ContactEmail { get; set; } //email adress for contact profile can be different that email used for authentication
        [MaxLength(500)]
        public string? ContactDescription { get; set; }

        public int CategoryId { get; set; }
        public virtual ContactCategory Category { get; set; } = new ContactCategory();
    }
}
