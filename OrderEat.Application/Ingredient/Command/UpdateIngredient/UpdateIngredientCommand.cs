using OrderEat.Application.Ingredient.Queries;
using MediatR;

namespace OrderEat.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommand : GetIngredientDto, IRequest
    {
    }
}