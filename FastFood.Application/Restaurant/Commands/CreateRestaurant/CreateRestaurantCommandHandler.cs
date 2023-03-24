using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, string>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IUserContextService _userContextService;

        public CreateRestaurantCommandHandler(IRestaurantRepository repository, IUserContextService userContextService)
        {
            _repository = repository;
            _userContextService = userContextService;
        }

        public async Task<string> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId;
            if (userId is null)
            {
                throw new BadRequestException("Invalid user token");
            }

            var newRestaurant = new Domain.Entities.Restaurant()
            {
                Name = request.Name,
                Description = request.Description,
                CreatedById = (int)userId,
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