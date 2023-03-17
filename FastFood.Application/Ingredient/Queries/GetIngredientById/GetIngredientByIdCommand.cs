using MediatR;

namespace FastFood.Application.Ingredient.Queries.GetIngredientById
{
    public class GetIngredientByIdCommand : IRequest<GetIngredientDto>
    {
        public int Id { get; set; }
    }
}