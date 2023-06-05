using MediatR;

namespace FastFood.Application.Dish.Command.CreateDish
{
    public class CreateDishCommand : GetDishDto, IRequest<string>
    {
        public int RestaurantId { get; set; }

    }
}