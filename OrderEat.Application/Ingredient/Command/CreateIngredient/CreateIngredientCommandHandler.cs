using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Ingredient.Command.CreateIngredient
{
    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, string>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;

        public CreateIngredientCommandHandler(IDishRepository dishRepository, IIngredientRepository ingredientRepository, IRestaurantRepository restaurantRepository, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _dishRepository = dishRepository;
            _ingredientRepository = ingredientRepository;
            _restaurantRepository = restaurantRepository;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public async Task<string> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var restaurant = await _restaurantRepository.GetById(dish.Id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not Found");
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            var newIngredient = new Domain.Entities.Ingredient()
            {
                Name = request.Name,
                Description = request.Description,
                IsRequired = request.IsRequired,
                Prize = request.Prize,
                Dish = dish
            };

            await _ingredientRepository.Create(newIngredient);

            return newIngredient.Id.ToString();
        }
    }
}