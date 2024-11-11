using Domain.Models;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;
using Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Enteties;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<(Result, string)> GenerateJwt(LoginUserDto loginUserDto);
        Task<Result> RegisterUser(RegisterUserDto registerUserDto);
    }

    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(DataContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        // Registers a new user in the system
        public async Task<Result> RegisterUser(RegisterUserDto registerUserDto)
        {
            // Create the user entity
            var user = new User
            {
                Email = registerUserDto.Email,
            };
            // Hash and set the password
            user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);
            // Save the user in the database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            //Create new user profile
            var userProfile = new UserProfile
            {
                UserId = user.Id,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                DateOfBirth = registerUserDto.DateOfBirth
            };
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        // Generates a JWT token for user authentication
        public async Task<(Result, string)> GenerateJwt(LoginUserDto loginUserDto)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);
            if (user == null)
            {
                return (Result.Failure(new Error("404", "Invalid email or password")), string.Empty);
            }

            // Verify the password
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return (Result.Failure(new Error("404", "Invalid email or password")), string.Empty);
            }

            // Create JWT token claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Generate security key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set token expiration date
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            // Create the JWT token
            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return (Result.Success(), jwtToken);
        }
    }
}
