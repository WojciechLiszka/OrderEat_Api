using AutoMapper;
using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Ingredient.Queries.GetIngredientById
{
    public class GetIngredientByIdCommandHandler : IRequestHandler<GetIngredientByIdCommand, GetIngredientDto>
    {
        private readonly IIngredientRepository _repository;
        private readonly IMapper _mapper;

        public GetIngredientByIdCommandHandler(IIngredientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetIngredientDto> Handle(GetIngredientByIdCommand request, CancellationToken cancellationToken)
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