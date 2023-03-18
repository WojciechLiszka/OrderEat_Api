using FastFood.Application.Ingredient.Queries;
using MediatR;

namespace FastFood.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommand : GetIngredientDto, IRequest
    {
    }
}