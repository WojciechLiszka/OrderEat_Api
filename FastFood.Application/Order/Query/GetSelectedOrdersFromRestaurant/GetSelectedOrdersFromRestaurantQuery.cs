using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Order.Query.GetOrdersToRealizeFromRestaurant
{
    public class GetSelectedOrdersRestaurantQuery :PagedResultDto, IRequest<PagedResult<Domain.Entities.Order>>
    {
        public int RestaurantId { get; set; }
        public OrderStatus SelectedStatus { get; set; }
    }
}