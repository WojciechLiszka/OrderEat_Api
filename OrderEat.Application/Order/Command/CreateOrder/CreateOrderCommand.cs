using MediatR;

namespace OrderEat.Application.Order.Command.CreateOrder
{
    public class CreateOrderCommand : IRequest<string>
    {
        public int RestaurantId { get; set; }
    }
}