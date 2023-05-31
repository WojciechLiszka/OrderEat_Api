using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FastFood.Application.Order.Command.RealizeOrder
{
    public class OrderOrderCommandHandler : IRequestHandler<OrderOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public OrderOrderCommandHandler(IOrderRepository orderRepository, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public async Task Handle(OrderOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(request.Orderid);

            if (order == null)
            {
                throw new NotFoundException();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, order, new OrderRsourceOperationRequirement(ResourceOperation.Update));
            if (order.OrderedDishes.IsNullOrEmpty())
            {
                throw new BadRequestException("You need to add dishes to order");
            }
            if (order.Status != Domain.Models.OrderStatus.InCart)
            {
                throw new BadRequestException("This order is already ordered");
            }
            order.OrderDate = DateTime.Now;
            order.Status = Domain.Models.OrderStatus.Ordered;
            await _orderRepository.Commit();
        }
    }
}