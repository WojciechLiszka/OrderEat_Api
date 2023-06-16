using AutoMapper;
using OrderEat.Application.Account;
using OrderEat.Domain.Entities;
using System.Security;

namespace OrderEat.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.ContactNumber, opt => opt.MapFrom(src => src.ContactDetails.ContactNumber))
                .ForMember(dto => dto.City, opt => opt.MapFrom(src => src.ContactDetails.City))
                .ForMember(dto => dto.Country, opt => opt.MapFrom(src => src.ContactDetails.Country))
                .ForMember(dto => dto.Street, opt => opt.MapFrom(src => src.ContactDetails.Street))
                .ForMember(dto => dto.ApartmentNumber, opt => opt.MapFrom(src => src.ContactDetails.ApartmentNumber));
        }
    }
}