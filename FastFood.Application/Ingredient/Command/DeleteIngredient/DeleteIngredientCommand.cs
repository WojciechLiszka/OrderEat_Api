using MediatR;

namespace FastFood.Application.Ingredient.Command.DeleteIngredient
{
    public class DeleteIngredientCommand : IRequest
    {
        public int Id { get; set; }
    }
}