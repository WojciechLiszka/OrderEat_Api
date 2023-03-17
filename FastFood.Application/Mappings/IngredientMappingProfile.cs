using AutoMapper;
using FastFood.Application.Ingredient.Queries;

namespace FastFood.Application.Mappings
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