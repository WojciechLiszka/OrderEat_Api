using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContext;

        public DeleteRestaurantCommandHandler(IRestaurantRepository repository, IAuthorizationService authorization, IUserContextService userContext)
        {
            _repository = repository;
            _authorization = authorization;
            _userContext = userContext;
        }

        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetById(request.Id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            var authorizationResult = await _authorization.AuthorizeAsync(_userContext.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Delete));
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            await _repository.Delete(restaurant);
        }
    }
}