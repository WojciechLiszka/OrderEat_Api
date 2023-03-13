using AutoMapper;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Allergen.Commands.CreateAllergen
{
    public class CreateAllergenCommandHandler : IRequestHandler<CreateAllergenCommand>
    {
        private readonly IMapper _mapper;
        private readonly IAllergenRepository _repository;

        public CreateAllergenCommandHandler(IMapper mapper, IAllergenRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Handle(CreateAllergenCommand request, CancellationToken cancellationToken)
        {
            var allergen = _mapper.Map<Domain.Entities.Allergen>(request);
            await _repository.Create(allergen);
        }
    }
}