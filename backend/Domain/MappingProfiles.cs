using AutoMapper;
using Domain.Enteties;
using Domain.Models;
namespace Domain
{
    // Configurations for AutoMapper, allowing DTOs to be mapped to domain entities.
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContactDto, Contact>()
               .ForPath(dest => dest.UserProfile.FirstName, opt => opt.MapFrom(src => src.FirstName))
               .ForPath(dest => dest.UserProfile.LastName, opt => opt.MapFrom(src => src.LastName))
               .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.ContactDescription, opt => opt.MapFrom(src => src.ContactDescription));

            CreateMap<Contact, ContactDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserProfile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserProfile.LastName))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.ContactDescription, opt => opt.MapFrom(src => src.ContactDescription));
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<CreateContactDto, Contact>();


        }
    }
}
