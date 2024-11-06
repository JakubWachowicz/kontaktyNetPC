using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enteties
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } //Foregin key to user

        public int MyPersonalContactProfilesId { get; set; } // Foregin key to personal contact profiles

        //List used for storing user personal contact profiles. 
        //can be null
        public virtual List<Contact>? MyPersonalContactProfiles { get; set; } 
        public int MyContactBookId { get; set; } //Foregin key to user contact list

        //List of other users contacts profiles
        //(Works like friend list,User can add other users profiles to he's cantact book)
        //can be null
        public virtual List<Contact>? MyContactBook { get; set; } 
    }
}
