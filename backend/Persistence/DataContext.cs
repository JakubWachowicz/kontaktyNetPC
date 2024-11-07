using Domain.Enteties;
using Microsoft.EntityFrameworkCore;
namespace Persistence
{
    public class DataContext :  DbContext
    {
        public DataContext(DbContextOptions options) : base(options){}
        //Table containing users credentials
        public DbSet<User> Users { get; set; }
        //Table containing user profile informations
        public DbSet<UserProfile> UserProfiles { get; set; }
        // Table containing contact profile informations
        public DbSet<Contact> Contacts { get; set; }

    }
}
