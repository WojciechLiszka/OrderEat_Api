using MediatR;

namespace FastFood.Application.Order.CreateOrder
{
    public class CreateOrderCommand : IRequest<string>
    {
        public int RestaurantId { get; set; }
    }
}