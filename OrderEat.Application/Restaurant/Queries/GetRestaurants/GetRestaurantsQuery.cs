using OrderEat.Application.Dish.Queries;
using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQuery :PagedResultDto, IRequest<PagedResult<GetRestaurantDto>>
    {
        
    }
}