using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Dish.Queries.GetDishById
{
    public class GetDishByIdQuery : IRequest<GetDishDto>
    {
        public int DishId { get; set; }
    }
}