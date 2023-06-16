using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Dish.Queries.SmartSearchDish
{
    public class SmartSearchDishFromRestaurantQuery : PagedResultDto, IRequest<PagedResult<GetDishDto>>
    {
        public int RestaurantId { get; set; }
    }
}