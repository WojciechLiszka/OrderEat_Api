using FastFood.Application.Dish.Queries;
using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQuery :PagedResultDto, IRequest<PagedResult<GetRestaurantDto>>
    {
        
    }
}