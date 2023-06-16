using AutoMapper;
using Domain.Domain.Exceptions;
using OrderEat.Application.Allergen.Commands.CreateAllergen;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.Allergen.Queries.GetAllergenById
{
    public class GetAllergenByIdQueryHandler : IRequestHandler<GetAllergenByIdQuery, AllergenDto>
    {
        private readonly IAllergenRepository _repository;
        private readonly IMapper _mapper;

        public GetAllergenByIdQueryHandler(IAllergenRepository repository, IMapper mapper)
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