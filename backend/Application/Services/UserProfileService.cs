using AutoMapper;
using Domain;
using Domain.Enteties;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
namespace Application.Services
{
    public interface IUserProfileService
    {
        Result AddContactToProfileBook(int id);
        (Result, UserProfileDto) GetUserProfile(int id);
        IEnumerable<ContactDto> GetYourContacts(int id);
        Result UpdateUserProfile(int id, UpdateUserProfileDto updateUserProfileDto);
    }
    //Service for profile specific operations

    public class UserProfileService : IUserProfileService
    {

        private DataContext _context;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public UserProfileService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        //Get profile by id
        public (Result, UserProfileDto) GetUserProfile(int id)
        {
            //Chceck if profile exists
            var userProfile = _context.UserProfiles.FirstOrDefault(u => u.UserId == id);
            if (userProfile == null)
            {
                return (Result.Failure(new Error("404", "User Profile not found")), new UserProfileDto());
            }
            //Map to dto
            var userProfileDto = _mapper.Map<UserProfileDto>(userProfile);

            return (Result.Success(), userProfileDto);
        }

        //Get contacts created by you
        public IEnumerable<ContactDto> GetYourContacts(int id)
        {
            var contacts = _context.Contacts.Include(c=>c.Category).Include(c => c.UserProfile).Where(u => u.UserProfile.UserId == id);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
        //Update your contact
        public Result UpdateUserProfile(int id, UpdateUserProfileDto updateUserProfileDto)
        {

            UserProfile userProfile = _context.UserProfiles.First(u => u.UserId == id);
            if (userProfile == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }
            //Profile mapping
            userProfile.FirstName = !string.IsNullOrEmpty(updateUserProfileDto.FirstName) ? updateUserProfileDto.FirstName : userProfile.FirstName;
            userProfile.LastName = !string.IsNullOrEmpty(updateUserProfileDto.LastName) ? updateUserProfileDto.LastName : userProfile.LastName;
            userProfile.DateOfBirth = updateUserProfileDto.DateOfBirth ?? userProfile.DateOfBirth;

            _context.SaveChanges();
            return Result.Success();
        }
        
        public Result AddContactToProfileBook(int id)
        {
            //Check claims and search for User Id
            Claim? currentLoggedUserId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentLoggedUserId == null)
            {
                return Result.Failure(new Error("403", "Unauthorize"));
            }
            if ( currentLoggedUserId.Value == id.ToString())
            {
                return Result.Failure(new Error("400", "You can't add yourself to contact book"));
            }
            //Contact user to add
            Contact? contact = _context.Contacts.FirstOrDefault(c=>c.Id == id);
            if (contact == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }
            UserProfile userProfile = _context.UserProfiles.Find(id)!;
            userProfile.Contacts.Add(contact);
            _context.SaveChanges();
            return Result.Success();
        }

    }
}
