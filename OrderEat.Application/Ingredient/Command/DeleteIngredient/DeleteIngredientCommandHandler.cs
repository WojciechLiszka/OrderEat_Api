using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Ingredient.Command.DeleteIngredient
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand>
    {
        private readonly IIngredientRepository _repository;
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;

        public DeleteIngredientCommandHandler(IIngredientRepository repository, IDishRepository dishRepository, IRestaurantRepository restaurantRepository, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _repository = repository;
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _repository.GetById(request.Id);
            if (ingredient == null)
            {
                throw new NotFoundException("Ingredient not found");
            }
            var dish = await _dishRepository.GetById(ingredient.Dish.Id);
            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            var restaurant = await _restaurantRepository.GetById(dish.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            await _repository.Delete(ingredient);
        }
    }
}