using Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Allergen.Commands.DeleteAllergen
{
    public class DeleteAllergenCommandHandler : IRequestHandler<DeleteAllergenCommand>
    {
        private readonly IFastFoodRepository _repository;

        public DeleteAllergenCommandHandler(IFastFoodRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteAllergenCommand request, CancellationToken cancellationToken)
        {
            var allergen = await _repository.GetById(request.Id);
            if (allergen == null)
            {
                throw new NotFoundException("Domain not found");
            }
            await _repository.Delete(allergen);
        }
    }
}