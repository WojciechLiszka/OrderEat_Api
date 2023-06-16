using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Order.Command.AddDishToOrder
{
    public class AddDishToOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public Sheet AditionalIngrediens { get; set; }
    }
}