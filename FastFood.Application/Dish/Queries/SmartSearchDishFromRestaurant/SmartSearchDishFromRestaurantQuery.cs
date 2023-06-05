using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Dish.Queries.SmartSearchDish
{
    public class SmartSearchDishFromRestaurantQuery : PagedResultDto, IRequest<PagedResult<GetDishDto>>
    {
        public int RestaurantId { get; set; }
    }
}