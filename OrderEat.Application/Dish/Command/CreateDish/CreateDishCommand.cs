using MediatR;

namespace OrderEat.Application.Dish.Command.CreateDish
{
    public class CreateDishCommand : GetDishDto, IRequest<string>
    {
        public int RestaurantId { get; set; }

    }
}