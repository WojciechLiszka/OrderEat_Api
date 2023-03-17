using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Allergen.Commands.DeleteAllergen
{
    public class DeleteAllergenCommandHandler : IRequestHandler<DeleteAllergenCommand>
    {
        private readonly IAllergenRepository _repository;

        public DeleteAllergenCommandHandler(IAllergenRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteAllergenCommand request, CancellationToken cancellationToken)
        {
            var allergen = await _repository.GetById(request.Id);
            if (allergen == null)
            {
                throw new NotFoundException("Allergen not found");
            }
            await _repository.Delete(allergen);
        }
    }
}