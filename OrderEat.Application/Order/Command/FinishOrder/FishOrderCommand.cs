using MediatR;

namespace OrderEat.Application.Order.Command.FinishOrder
{
    public class FishOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}