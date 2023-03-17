using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Dish.Command.CreateDish
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, string>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public CreateDishCommandHandler(IDishRepository dishRepository, IRestaurantRepository restaurantRepository)
        {
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<string> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetById(request.RestaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var newDish = new Domain.Entities.Dish()
            {
                Restaurant = restaurant,
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