using AutoMapper;
using OrderEat.Application.Allergen.Commands.CreateAllergen;

namespace OrderEat.Application.Mappings
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