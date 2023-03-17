using AutoMapper;
using FastFood.Application.Dish.Queries;

namespace FastFood.Application.Mappings
{
    public class DishMappingProfile : Profile
    {
        public DishMappingProfile()
        {
            CreateMap<Domain.Entities.Dish, GetDishDto>()
                .ReverseMap();
        }
    }
}