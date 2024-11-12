using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int UserId { get; set; }

    }

}
