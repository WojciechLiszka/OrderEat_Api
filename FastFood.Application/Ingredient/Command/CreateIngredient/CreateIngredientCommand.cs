using MediatR;

namespace FastFood.Application.Ingredient.Command.CreateIngredient
{
    public class CreateIngredientCommand : IngredientDto, IRequest<string>
    {
        public int DishId { get; set; }
    }
}