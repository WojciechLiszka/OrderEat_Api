using AutoMapper;
using OrderEat.Application.Dish.Queries;

namespace OrderEat.Application.Mappings
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