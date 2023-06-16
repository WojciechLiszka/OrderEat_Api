using MediatR;

namespace OrderEat.Application.Ingredient.Command.CreateIngredient
{
    public class CreateIngredientCommand : IngredientDto, IRequest<string>
    {
        public int DishId { get; set; }
    }
}