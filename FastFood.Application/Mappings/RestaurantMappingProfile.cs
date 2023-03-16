using AutoMapper;
using FastFood.Application.Restaurant;
using FastFood.Application.Restaurant.Queries;

namespace FastFood.Application.Mappings
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Domain.Entities.Restaurant, GetRestaurantDto>()
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.ContactDetails.Email))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.ContactDetails.Country))
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.ContactDetails.City))
                .ForMember(d => d.Street, opt => opt.MapFrom(src => src.ContactDetails.Street))
                .ForMember(d => d.ApartmentNumber, opt => opt.MapFrom(src => src.ContactDetails.ApartmentNumber));
        }
    }
}