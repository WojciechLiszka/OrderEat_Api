using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Ingredient.Queries.GetIngredientsFromDish
{
    public class GetIngredientsFromDishQuery : IRequest<List<GetIngredientDto>>
    {
        public int DishId { get; set; }
    }
}