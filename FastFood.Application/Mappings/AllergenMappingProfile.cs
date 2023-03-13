using AutoMapper;
using FastFood.Application.Allergen.Commands.CreateAllergen;

namespace FastFood.Application.Mappings
{
    public class AllergenMappingProfile : Profile
    {
        public AllergenMappingProfile()
        {
            CreateMap<Domain.Entities.Allergen, CreateAllergenCommand>()
                .ReverseMap();
        }
    }
}