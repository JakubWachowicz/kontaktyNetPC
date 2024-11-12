using Application.Services;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet, Authorize]
        public ActionResult<UserProfileDto> Get()
        {
            var currentLoggedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var (result, userProfile) = _userProfileService.GetUserProfile(int.Parse(currentLoggedUserId));
            if (result.IsFailure)
            {
                return NotFound(result.Error.Description);
            }

            return Ok(userProfile);
        }
        [HttpGet("your-contacts"),Authorize]
        public ActionResult<IEnumerable<ContactDto>> GetYourContacts()
        {
            var currentLoggedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var contacts = _userProfileService.GetYourContacts(int.Parse(currentLoggedUserId));
            return Ok(contacts);
        }

        [HttpGet("{id}"),Authorize]
        public ActionResult<UserProfileDto> Get(int id)
        {

            var (result, userProfile) = _userProfileService.GetUserProfile(id);
            if (result.IsFailure)
            {
                return NotFound(result.Error.Description);
            }

            return Ok(userProfile);
        }

        [HttpPut, Authorize]
        public ActionResult Update([FromBody] UpdateUserProfileDto userProfileDto)
        {
            var currentLoggedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Result result = _userProfileService.UpdateUserProfile(int.Parse(currentLoggedUserId),userProfileDto);
            if (result.IsFailure)
            {
                BadRequest(result.Error.Description);
            }
            return Ok();
        }

        [HttpPut("{id}"), Authorize]
        public ActionResult AddProfileToContactBook([FromBody] int id)
        {
            Result result = _userProfileService.AddContactToProfileBook(id);
            if (result.IsFailure)
            {
                return BadRequest(result.Error.Description);
            }
            return Ok();
        }

    }
}
