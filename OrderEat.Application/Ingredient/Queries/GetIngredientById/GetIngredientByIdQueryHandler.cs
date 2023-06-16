using AutoMapper;
using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.Ingredient.Queries.GetIngredientById
{
    public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, GetIngredientDto>
    {
        private readonly IIngredientRepository _repository;
        private readonly IMapper _mapper;

        public GetIngredientByIdQueryHandler(IIngredientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetIngredientDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
        {
            var ingredient = await _repository.GetById(request.Id);

            if (ingredient == null)
            {
                throw new NotFoundException("Ingredient not found");
            }

            var dto = _mapper.Map<GetIngredientDto>(ingredient);
            return dto;
        }
    }
}