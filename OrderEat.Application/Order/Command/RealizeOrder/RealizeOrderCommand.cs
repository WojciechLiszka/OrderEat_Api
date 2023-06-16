using MediatR;

namespace OrderEat.Application.Order.Command.RealizeOrder
{
    public class RealizeOrderCommand :IRequest
    {
        public int Orderid { get; set; }
    }
}