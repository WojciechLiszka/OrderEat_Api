using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;

        public UpdateIngredientCommandHandler(IIngredientRepository ingredientRepository, IDishRepository dishRepository, IRestaurantRepository restaurantRepository, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _ingredientRepository = ingredientRepository;
            _dishRepository = dishRepository;
            _ingredientRepository = ingredientRepository;
            _restaurantRepository = restaurantRepository;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientRepository.GetById(request.Id);
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

            ingredient.Name = request.Name;
            ingredient.Description = request.Description;
            ingredient.Prize = request.Prize;
            ingredient.IsRequired = request.IsRequired;

            await _ingredientRepository.Commit();
        }
    }
}