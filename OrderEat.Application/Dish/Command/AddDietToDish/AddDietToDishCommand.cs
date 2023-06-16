using MediatR;

namespace OrderEat.Application.Dish.Command.AddDietToDish
{
    public class AddDietToDishCommand : IRequest
    {
        public int DishId { get; set; }
        public int DietId { get; set; }
    }
}