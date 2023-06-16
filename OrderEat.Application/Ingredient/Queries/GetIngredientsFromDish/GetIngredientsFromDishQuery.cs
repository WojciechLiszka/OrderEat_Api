using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Ingredient.Queries.GetIngredientsFromDish
{
    public class GetIngredientsFromDishQuery : IRequest<List<GetIngredientDto>>
    {
        public int DishId { get; set; }
    }
}