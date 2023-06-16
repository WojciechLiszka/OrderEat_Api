using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Dish.Command.UpdateDish
{
    public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;
        private readonly IRestaurantRepository _restaurantRepository;

        public UpdateDishCommandHandler(IDishRepository dishRepository, IAuthorizationService authorizationService, IUserContextService userContext,IRestaurantRepository restaurantRepository)
        {
            _dishRepository = dishRepository;
            _authorizationService = authorizationService;
            _userContext = userContext;
            _restaurantRepository = restaurantRepository;
        }

        public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            var restaurant = await _restaurantRepository.GetById(dish.RestaurantId);
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }
            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.AllowedCustomization = request.AllowedCustomization;
            dish.BasePrize = request.BasePrize;
            dish.BaseCaloricValue = request.BaseCaloricValue;
            dish.IsAvilable = request.IsAvilable;

            await _dishRepository.Commit();
        }
    }
}