using Domain.Enteties;
using Domain.Entities;
using Persistence;

namespace Application.Services
{
    //Service for seeding mock data to db
    public class DbSeederService
    {
        private readonly DataContext _context;

        public DbSeederService(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Users.Any())
            {
                var users = SeedUsers();
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
            if (!_context.Categories.Any())
            {
                var categories = SeedCategories();
                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        private List<Category> SeedCategories()
        {

            return new List<Category>
            {
                new Category
                {
                    Name = "Private",
                      SubCategories = new List<string>
                        {
                          "Friend",
                          "Family",
                          "Hobby"
                       }
                },
                new Category
                {
                    Name ="Business",
                    SubCategories =new List<string>
                        {
                          "boss","client"
                       }
                },
                 new Category
                {
                    Name ="Other",
                },
            };
        }
        //Create sample users
        //TODO: Seed profile categories and contacts
        private List<User> SeedUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Email = "johndoe@example.com",
                    PasswordHash = "hashedPassword1",

                },
                new User
                {
                    Email = "janesmith@example.com",
                    PasswordHash = "hashedPassword2",
                }
            };

            return users;
        }
    }
}
