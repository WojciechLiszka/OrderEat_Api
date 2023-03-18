using AutoMapper;
using FastFood.Application.SpecialDiet.Queries;

namespace FastFood.Application.Mappings
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