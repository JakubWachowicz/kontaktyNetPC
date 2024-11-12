
using System.ComponentModel.DataAnnotations;

public class ContactDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int UserProfileId { get; set; }  // ID of the contact's owner

    public string? PhoneNumber { get; set; }

    [Required, EmailAddress]
    public string? ContactEmail { get; set; } // Can be different from the user's main authentication email

    [MaxLength(500)]
    public string? ContactDescription { get; set; }

    public string? CategoryName { get; set; } // Display the category name instead of full navigation property
    public string? SubCategory { get; set; }
}