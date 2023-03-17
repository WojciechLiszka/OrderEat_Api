using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Ingredient.Command.DeleteIngredient
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand>
    {
        private readonly IIngredientRepository _repository;

        public DeleteIngredientCommandHandler(IIngredientRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _repository.GetById(request.Id);
            if (ingredient == null)
            {
                throw new NotFoundException("Ingredient not found");
            }
            await _repository.Delete(ingredient);
        }
    }
}