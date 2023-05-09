using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Order.Command.AddDishToOrder
{
    public class AddDishToOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public Sheet AditionalIngrediens { get; set; }
    }
}