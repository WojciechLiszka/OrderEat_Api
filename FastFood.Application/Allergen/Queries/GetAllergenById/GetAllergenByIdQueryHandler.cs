using AutoMapper;
using Domain.Exceptions;
using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Allergen.Queries.GetAllergenById
{
    public class GetAllergenByIdQueryHandler : IRequestHandler<GetAllergenByIdQuery, AllergenDto>
    {
        private readonly IFastFoodRepository _repository;
        private readonly IMapper _mapper;

        public GetAllergenByIdQueryHandler(IFastFoodRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AllergenDto> Handle(GetAllergenByIdQuery request, CancellationToken cancellationToken)
        {
            var allergen = await _repository.GetById(request.Id);
            if (allergen == null)
            {
                throw new NotFoundException("Allergen not found");
            }
            var dto = _mapper.Map<AllergenDto>(allergen);
            return dto;
        }
    }
}