using AutoMapper;
using Domain;
using Domain.Enteties;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Persistence;
using System.Security.Claims;
namespace Application.Services
{
    public interface IUserProfileService
    {
        Result AddContactToProfileBook(int id);
        (Result, UserProfileDto) GetUserProfile(int id);
        Result UpdateUserProfile(int id, UpdateUserProfileDto updateUserProfileDto);
    }

    public class UserProfileService : IUserProfileService
    {

        private DataContext _context;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public UserProfileService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
        }

        public (Result, UserProfileDto) GetUserProfile(int id)
        {
            var userProfile = _context.UserProfiles.FirstOrDefault(u => u.UserId == id);
            if (userProfile == null)
            {
                return (Result.Failure(new Error("404", "User Profile not found")), new UserProfileDto());
            }

            var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);

            return (Result.Success(), userProfileDto);
        }

        public Result UpdateUserProfile(int id, UpdateUserProfileDto updateUserProfileDto)
        {

            UserProfile userProfile = _context.UserProfiles.First(u => u.UserId == id);
            if (userProfile == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }

            userProfile.FirstName = !string.IsNullOrEmpty(updateUserProfileDto.FirstName) ? updateUserProfileDto.FirstName : userProfile.FirstName;
            userProfile.LastName = !string.IsNullOrEmpty(updateUserProfileDto.LastName) ? updateUserProfileDto.LastName : userProfile.LastName;
            userProfile.DateOfBirth = updateUserProfileDto.DateOfBirth ?? userProfile.DateOfBirth;

            _context.SaveChanges();
            return Result.Success();
        }

        public Result AddContactToProfileBook(int id)
        {
            var currentLoggedUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentLoggedUserId.Value == id.ToString())
            {

                return Result.Failure(new Error("400", "You can't add yourself to contact book"));
            }
            //Contact user to add
            Contact contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }
            UserProfile userProfile = _context.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }
            userProfile.Contacts.Add(contact);
            _context.SaveChanges();
            return Result.Success();
        }

    }
}
