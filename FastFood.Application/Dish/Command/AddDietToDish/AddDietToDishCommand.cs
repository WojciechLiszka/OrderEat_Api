using MediatR;

namespace FastFood.Application.Dish.Command.AddDietToDish
{
    public class AddDietToDishCommand : IRequest
    {
        public int DishId { get; set; }
        public int DietId { get; set; }
    }
}