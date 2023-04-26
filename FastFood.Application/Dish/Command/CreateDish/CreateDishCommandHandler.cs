using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Dish.Command.CreateDish
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, string>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContext;

        public CreateDishCommandHandler(IDishRepository dishRepository, IRestaurantRepository restaurantRepository, IAuthorizationService authorization, IUserContextService userContext)
        {
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
            _authorization = authorization;
            _userContext = userContext;
        }

        public async Task<string> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetById(request.RestaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResult = await _authorization.AuthorizeAsync(_userContext.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            var newDish = new Domain.Entities.Dish()
            {
                RestaurantId = restaurant.Id,
                Name = request.Name,
                Description = request.Description,
                BasePrize = request.BasePrize,
                BaseCaloricValue = request.BaseCaloricValue,
                IsAvilable = request.IsAvilable,
                AllowedCustomization = request.AllowedCustomization
            };

            await _dishRepository.Create(newDish);
            return newDish.Id.ToString();
        }
    }
}