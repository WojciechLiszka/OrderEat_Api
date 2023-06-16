using AutoMapper;
using OrderEat.Application.Ingredient.Queries;

namespace OrderEat.Application.Mappings
{
    public class IngredientMappingProfile : Profile
    {
        public IngredientMappingProfile()
        {
            CreateMap<Domain.Entities.Ingredient, GetIngredientDto>()
                .ReverseMap();
        }
    }
}