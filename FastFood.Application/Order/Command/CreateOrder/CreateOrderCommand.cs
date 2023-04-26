using MediatR;

namespace FastFood.Application.Order.Command.CreateOrder
{
    public class CreateOrderCommand : IRequest<string>
    {
        public int RestaurantId { get; set; }
    }
}