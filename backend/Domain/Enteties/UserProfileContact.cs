using System.ComponentModel.DataAnnotations;

namespace Domain.Enteties
{
    public class UserProfileContact
    {
        [Key]
        public int Id { get; set; }

        // PK to user profile
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        // FK to Contact
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }

}
