using AutoMapper;
using OrderEat.Application.SpecialDiet.Queries;

namespace OrderEat.Application.Mappings
{
    public class SpecialDietMappingProfile : Profile
    {
        public SpecialDietMappingProfile()
        {
            CreateMap<Domain.Entities.SpecialDiet, GetDietDto>()
                .ReverseMap();
        }
    }
}