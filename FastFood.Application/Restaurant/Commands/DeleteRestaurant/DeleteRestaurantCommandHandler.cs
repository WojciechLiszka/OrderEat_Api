using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
    {
        private readonly IRestaurantRepository _repository;

        public DeleteRestaurantCommandHandler(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetById(request.Id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            await _repository.Delete(restaurant);
        }
    }
}