using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Order.Query.GetOrdersToRealizeFromRestaurant
{
    public class GetOrdersToRealizeFromRestaurantQuery :PagedResultDto, IRequest<PagedResult<Domain.Entities.Order>>
    {
        public int RestaurantId { get; set; }
    }
}