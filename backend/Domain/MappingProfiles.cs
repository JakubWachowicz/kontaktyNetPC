using AutoMapper;
using Domain.Enteties;
using Domain.Entities;
using Domain.Models;
namespace Domain
{
    // Configurations for AutoMapper, allowing DTOs to be mapped to domain entities.
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContactDto, Contact>()
               .ForPath(dest => dest.UserProfile.FirstName, opt => opt.MapFrom(src => src.FirstName))
               .ForPath(dest => dest.UserProfile.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.UserProfile.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForPath(dest => dest.Category.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForPath(dest => dest.Category.SubcategoryName, opt => opt.MapFrom(src => src.SubCategory))
               .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.ContactDescription, opt => opt.MapFrom(src => src.ContactDescription));

            CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.UserProfile.DateOfBirth))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.ContactDescription, opt => opt.MapFrom(src => src.ContactDescription));
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();

            CreateMap<CreateContactDto, Contact>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.ContactDescription, opt => opt.MapFrom(src => src.ContactDescription))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail));

            CreateMap<CategoryDto, Category>()
             .ForPath(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubcategoryName))
             .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Contact, CreateContactDto>();




        }
    }
}
