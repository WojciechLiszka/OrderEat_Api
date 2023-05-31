using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Order.Command.RealizeOrder
{
    public class OrderOrderCommand : IRequest
    {
        public int Orderid { get; set; }
        
    }
}