using FastFood.Domain.Interfaces;
using MediatR;


namespace FastFood.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, string>
    {
        private readonly IRestaurantRepository _repository;

        public CreateRestaurantCommandHandler(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var newRestaurant = new Domain.Entities.Restaurant()
            {
                Name = request.Name,
                Description = request.Description,
                ContactDetails = new Domain.Entities.RestaurantContactDetails()
                {
                    ContactNumber = request.ContactNumber,
                    Email = request.Email,
                    Country = request.Country,
                    City = request.City,
                    Street = request.Street,
                    ApartmentNumber = request.ApartmentNumber
                }
            };

            await _repository.Create(newRestaurant);

            return newRestaurant.Id.ToString();
        }
    }
}