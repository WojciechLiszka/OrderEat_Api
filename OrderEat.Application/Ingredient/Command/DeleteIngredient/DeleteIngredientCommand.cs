using MediatR;

namespace OrderEat.Application.Ingredient.Command.DeleteIngredient
{
    public class DeleteIngredientCommand : IRequest
    {
        public int Id { get; set; }
    }
}