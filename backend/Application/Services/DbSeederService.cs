using Domain.Enteties;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
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
                var categories = SeedCategories();
                var users = SeedUsers(categories);

                await _context.AddRangeAsync(categories);
                await _context.AddRangeAsync(users);
                await _context.SaveChangesAsync();
            }
        }

        private List<Category> SeedCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Name = CategoryName.Private,
                    SubCategories = new List<string> { "Friend", "Family", "Acquaintance" }
                },
                new Category
                {
                    Name = CategoryName.Buissnes,
                    SubCategories = new List<string> { "Boss", "Client" }
                },
                new Category
                {
                    Name = CategoryName.Other,
                    SubCategories = new List<string> {}
                }
            };
        }

        private List<User> SeedUsers(List<Category> categories)
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
