using MediatR;

namespace OrderEat.Application.Ingredient.Queries.GetIngredientById
{
    public class GetIngredientByIdQuery : IRequest<GetIngredientDto>
    {
        public int Id { get; set; }
    }
}