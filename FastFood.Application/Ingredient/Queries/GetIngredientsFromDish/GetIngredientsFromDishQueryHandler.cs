using AutoMapper;
using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Ingredient.Queries.GetIngredientsFromDish
{
    public class GetIngredientsFromDishQueryHandler : IRequestHandler<GetIngredientsFromDishQuery, List<GetIngredientDto>>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public GetIngredientsFromDishQueryHandler(IDishRepository dishRepository, IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<List<GetIngredientDto>> Handle(GetIngredientsFromDishQuery request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetByIdWithIngredients(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var dtos = _mapper.Map<List<GetIngredientDto>>(dish.AllowedIngreedients);

            return dtos;
        }
    }
}