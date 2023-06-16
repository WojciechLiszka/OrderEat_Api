using MediatR;

namespace OrderEat.Application.Dish.Command.UpdateDish
{
    public class UpdateDishCommand :GetDishDto,IRequest
    {
        public int DishId { get; set; }
    }
}