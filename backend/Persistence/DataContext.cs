using Domain.Enteties;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        //Table containing users credentials
        public DbSet<User> Users { get; set; }
        //Table containing user profile informations
        public DbSet<UserProfile> UserProfiles { get; set; }
        //Table containing contact profile informations
        public DbSet<Contact> Contacts { get; set; }
        //Table containging profile categories with their subcategories
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile) // User has one UserProfile
                .WithOne(up => up.User) // UserProfile has one User
                .HasForeignKey<UserProfile>(up => up.UserId) // Explicit foreign key in UserProfile
                .OnDelete(DeleteBehavior.Cascade); // Optionally set cascading delete behavior
            base.OnModelCreating(modelBuilder);
        }
    }
}
