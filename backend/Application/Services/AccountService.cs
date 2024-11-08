using Domain.Enteties;
using Domain.Models ;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Application.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginUserDto loginUserDto);
        void RegisterUser(RegisterUserDto registerUserDto);
    }

    public class AccountService : IAccountService
    {
        private readonly DataContext context;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(DataContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            User user = new User() { Email = registerUserDto.Email, };
            var hashedPassword = passwordHasher.HashPassword(user, registerUserDto.Password);
            user.PasswordHash = hashedPassword;
            context.Users.Add(user);
            context.SaveChanges();
        }

        //JWT Token gereration
        public string GenerateJwt(LoginUserDto loginUserDto)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == loginUserDto.Email);
            if (user == null)
            {
                //throw new BadCredentialsException("Invalid email or password");
            }
            var hashedPassword = passwordHasher.HashPassword(user, loginUserDto.Password);
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, hashedPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                //throw new BadCredentialsException("Invalid email or password");
            }
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email), };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer, authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

    }
}
