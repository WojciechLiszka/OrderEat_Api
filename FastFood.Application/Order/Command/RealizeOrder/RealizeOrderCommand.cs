using MediatR;

namespace FastFood.Application.Order.Command.RealizeOrder
{
    public class RealizeOrderCommand :IRequest
    {
        public int Orderid { get; set; }
    }
}