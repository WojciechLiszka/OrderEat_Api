using MediatR;

namespace FastFood.Application.Ingredient.Queries.GetIngredientById
{
    public class GetIngredientByIdQuery : IRequest<GetIngredientDto>
    {
        public int Id { get; set; }
    }
}