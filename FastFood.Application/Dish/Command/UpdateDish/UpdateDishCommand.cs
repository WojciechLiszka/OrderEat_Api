using MediatR;

namespace FastFood.Application.Dish.Command.UpdateDish
{
    public class UpdateDishCommand :DishDto,IRequest
    {
        public int DishId { get; set; }
    }
}