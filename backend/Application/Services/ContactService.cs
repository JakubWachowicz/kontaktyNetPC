
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Domain.Enteties;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain;

namespace Application.Services
{
    public interface IContactService
    {
        void CreateContact(int id, CreateContactDto contactDto);
        Result DeleteContact(int id);
        IEnumerable<ContactDto> GetAllContacts();
        Result UpdateContact(int id, ContactDto contactDto);
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
            var contacts = _context.Contacts;

            return _mapper.Map<List<ContactDto>>(contacts);
        }
        //Creating new contact
        public void CreateContact(int id,CreateContactDto contactDto)
        {
            if (id != null)
            {
                UserProfile? userProfile = _context.UserProfiles.FirstOrDefault(up => up.UserId == id);
                if (userProfile == null)
                {
                    //TODO: w przypadku null przypisz nw profil użytkownika
                }
                var newContact = _mapper.Map<Contact>(contactDto);
                newContact.UserProfileId = userProfile!.Id;
                if (newContact != null)
                {
                    _context.Contacts.Add(newContact);
                }
            }
        }

        public Result UpdateContact(int id, ContactDto contactDto)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return Result.Failure(new Error("404", "User not found"));
            }
            _mapper.Map<Contact>(contact);
            _context.SaveChanges();
            return Result.Success();

        }

        public Result DeleteContact(int id)
        {
            var contactToRemove = _context.Contacts.Find(id);
            if (contactToRemove == null)
            {
                return Result.Failure(new Error("404", "Contact not found"));
            }
            _context.Contacts.Remove(contactToRemove);
            _context.SaveChanges();
            return Result.Success();
        }
    }
}
