using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
    {
        private readonly IRestaurantRepository _repository;

        public UpdateRestaurantCommandHandler(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetById(request.Id);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
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