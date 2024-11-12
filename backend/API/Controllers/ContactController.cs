using Application.Services;
using Domain;
using Domain.Enteties;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {   
        //Injected service for contct related operations
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        //Get all users, authenticetion not required
        [HttpGet]
        public ActionResult<ContactDto> GetAll()
        {
            IEnumerable<ContactDto> contacts = _contactService.GetAllContacts();
            return Ok(contacts);
        }


        //Get user with specified id 
        //TODO: Change name to Get
        [HttpGet("{id}")]
        public ActionResult<ContactDto> GetAll(int id)
        {
            var contactDto = _contactService.GetContactById(id);
            return Ok(contactDto);
        }


        //Create new contact authenictaion required
        [Authorize]
        [HttpPost]
        public ActionResult CreateContact([FromBody] CreateContactDto contactDto)
        {
            var currentLoggedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _contactService.CreateContact(int.Parse(currentLoggedUserId),contactDto);
            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateContact([FromRoute]int id,[FromBody] CreateContactDto contactDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _contactService.UpdateContact(id,contactDto);
            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id) {

            var currentLoggedUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Result result = _contactService.DeleteContact(int.Parse(currentLoggedUserId),id);
            return Ok();
        }
    }
}
