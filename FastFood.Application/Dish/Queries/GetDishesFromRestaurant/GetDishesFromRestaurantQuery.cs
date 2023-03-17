using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Dish.Queries.GedDishesFromRestaurant
{
    public class GetDishesFromRestaurantQuery : PagedResultDto, IRequest<PagedResult<GetDishDto>>
    {
        public int RestaurantId { get; set; }
        
    }
}