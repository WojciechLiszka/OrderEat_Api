using Domain.Domain.Exceptions;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly IRestaurantRepository _restaurantRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUserContextService userContextService, IRestaurantRepository restaurantRepository)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _restaurantRepository = restaurantRepository;
        }

        async Task<string> IRequestHandler<CreateOrderCommand, string>.Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetById(request.RestaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not Found");
            }
            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new ForbiddenException();
            };

            var order = new Domain.Entities.Order()
            {
                UserId = (int)userId,
                RestaurantId = request.RestaurantId,
            };

            await _orderRepository.Create(order);
            return order.Id.ToString();
        }
    }
}