using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public UpdateRestaurantCommandHandler(IRestaurantRepository repository, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _repository = repository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetById(request.Id);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            var authorizationresult = await _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationresult.Succeeded)
            {
                throw new ForbiddenException();
            }

            restaurant.Description = request.Description;

            restaurant.ContactDetails.Email = request.Email;
            restaurant.ContactDetails.Country = request.Country;
            restaurant.ContactDetails.City = request.City;
            restaurant.ContactDetails.Street = request.Street;
            restaurant.ContactDetails.ApartmentNumber = request.ApartmentNumber;
            restaurant.ContactDetails.ContactNumber = request.ContactNumber;

            await _repository.Commit();
        }
    }
}