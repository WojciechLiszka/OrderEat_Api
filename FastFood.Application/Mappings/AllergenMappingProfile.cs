using AutoMapper;
using FastFood.Application.Allergen.Commands.CreateAllergen;

namespace FastFood.Application.Mappings
{
    public class AllergenMappingProfile : Profile
    {
        public AllergenMappingProfile()
        {
            CreateMap<CreateAllergenCommand, Domain.Entities.Allergen>();

            CreateMap<AllergenDto, Domain.Entities.Allergen>()
                .ReverseMap();

                
        }
    }
}