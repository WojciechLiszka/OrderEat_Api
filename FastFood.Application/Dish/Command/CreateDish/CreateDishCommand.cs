using MediatR;

namespace FastFood.Application.Dish.Command.CreateDish
{
    public class CreateDishCommand : DishDto, IRequest<string>
    {
        public int RestaurantId { get; set; }

    }
}