using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Dish.Queries.GedDishesFromRestaurant
{
    public class GetDishesFromRestaurantQuery : PagedResultDto, IRequest<PagedResult<GetDishDto>>
    {
        public int RestaurantId { get; set; }
        
    }
}