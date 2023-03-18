using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public UpdateIngredientCommandHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientRepository.GetById(request.Id);
            if (ingredient == null)
            {
                throw new NotFoundException("Ingredient not found");
            }

            ingredient.Name = request.Name;
            ingredient.Description = request.Description;
            ingredient.Prize = request.Prize;
            ingredient.IsRequired = request.IsRequired;

            await _ingredientRepository.Commit();
        }
    }
}