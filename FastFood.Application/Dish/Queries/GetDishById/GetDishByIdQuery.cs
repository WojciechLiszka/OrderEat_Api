using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Dish.Queries.GetDishById
{
    public class GetDishByIdQuery : IRequest<GetDishDto>
    {
        public int DishId { get; set; }
    }
}