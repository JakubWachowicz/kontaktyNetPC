
using AutoMapper;
using Domain;
using Domain.Enteties;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services
{
    public interface IContactService
    {
        void CreateContact(int id, CreateContactDto contactDto);
        Result DeleteContact(int currentLoggedUserId, int id);
        IEnumerable<ContactDto> GetAllContacts();
        ContactDto GetContactById(int id);
        Result UpdateContact(int id, CreateContactDto contactDto);
    }


    //service for contact releted operations
    public class ContactService : IContactService
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }
        //Get all contacts
        public IEnumerable<ContactDto> GetAllContacts()
        {
            var contacts = _context.Contacts.Include(c => c.UserProfile).Include(c => c.Category);
            return _mapper.Map<List<ContactDto>>(contacts);
        }
        public ContactDto GetContactById(int id)
        {
            var contacts = _context.Contacts.Include(c => c.UserProfile).Include(c => c.Category).FirstOrDefault(c => c.Id == id);
            if (contacts != null)
            {
                return _mapper.Map<ContactDto>(contacts);
            }
            return new ContactDto { };
        }

        //Creating new contact
        public void CreateContact(int id, CreateContactDto contactDto)
        {
            UserProfile? userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserId == id);
            var newContact = _mapper.Map<Contact>(contactDto);
            newContact.UserProfileId = userProfile!.Id;
            if (newContact != null)
            {
                _context.Contacts.Add(newContact);
                _context.SaveChanges();
            }
        }
        //Update contact
        public Result UpdateContact(int id, CreateContactDto contactDto)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }

            //Map contact
            contact.PhoneNumber = contactDto.PhoneNumber;
            contact.ContactDescription = contact.ContactDescription;
            contact.Category = new ContactCategory();
            contact.Category.Name = contactDto.Category.Name;
            contact.Category.SubcategoryName = contactDto.Category.SubcategoryName;
            contact.ContactEmail = contactDto.ContactEmail;
            _context.SaveChanges();
            return Result.Success();
        }

        public Result DeleteContact(int currentLoggedUserId, int id)
        {
            var contactToRemove = _context.Contacts
             .Include(c => c.UserProfile)
             .FirstOrDefault(c => c.Id == id);
            if (contactToRemove == null)
            {
                return Result.Failure(new Error("404", "Contact not found"));
            }
            if (contactToRemove.UserProfile.UserId == currentLoggedUserId)
            {
                _context.Contacts.Remove(contactToRemove);
                _context.SaveChanges();
            }
            else
            {
                return Result.Failure(new Error("400", "You can only delete contacts created by yourself"));
            }
            return Result.Success();
        }
    }
}
