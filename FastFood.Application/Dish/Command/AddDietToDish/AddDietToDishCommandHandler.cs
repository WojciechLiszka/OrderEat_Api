using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Dish.Command.AddDietToDish
{
    public class AddDietToDishCommandHandler : IRequestHandler<AddDietToDishCommand>
    {
        private readonly IDishRepository _dishRepository;
        private readonly ISpecialDietRepository _specialDietRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContext;
        private readonly IRestaurantRepository _restaurantRepository;

        public AddDietToDishCommandHandler(IDishRepository dishRepository, ISpecialDietRepository specialDietRepository, IAuthorizationService authorization, IUserContextService userContext, IRestaurantRepository restaurantRepository)
        {
            _dishRepository = dishRepository;
            _specialDietRepository = specialDietRepository;
            _authorization = authorization;
            _userContext = userContext;
            _restaurantRepository = restaurantRepository;
        }

        public async Task Handle(AddDietToDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetByIdWithAllowedDiets(request.DishId);
            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            var restaurant = _restaurantRepository.GetById(dish.RestaurantId);
            var authorizationResult = await _authorization.AuthorizeAsync(_userContext.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            var diet = await _specialDietRepository.GetById(request.DietId);

            if (diet == null)
            {
                throw new NotFoundException("Diet not found");
            }

            dish.AllowedForDiets.Add(diet);

            await _dishRepository.Commit();
        }
    }
}