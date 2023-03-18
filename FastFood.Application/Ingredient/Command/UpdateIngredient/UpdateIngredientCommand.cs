using MediatR;

namespace FastFood.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommand : IngredientDto, IRequest
    {
        public int Id { get; set; }
    }
}