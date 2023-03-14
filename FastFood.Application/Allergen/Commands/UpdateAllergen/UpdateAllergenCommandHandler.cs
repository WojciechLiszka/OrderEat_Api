using Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Allergen.Commands.UpdateAllergen
{
    public class UpdateAllergenCommandHandler : IRequestHandler<UpdateAllergenCommand>
    {
        private readonly IAllergenRepository _allergenrepository;

        public UpdateAllergenCommandHandler(IAllergenRepository allergenrepository)
        {
            _allergenrepository = allergenrepository;
        }

        public async Task Handle(UpdateAllergenCommand request, CancellationToken cancellationToken)
        {
            var allergen = await _allergenrepository.GetById(request.Id);

            if (allergen == null)
            {
                throw new NotFoundException("Allergen not found");
            }

            allergen.Name = request.Name;
            allergen.Description = request.Description;
            await _allergenrepository.Commit();

            return;
        }
    }
}