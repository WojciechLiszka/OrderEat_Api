using Domain.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;

namespace OrderEat.Application.Order.Command.FinishOrder
{
    public class FishOrderCommandHandler : IRequestHandler<FishOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public FishOrderCommandHandler(IOrderRepository orderRepository, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public async Task Handle(FishOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(request.Id);
            if (order == null)
            {
                throw new NotFoundException();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, order, new OrderRsourceOperationRequirement(ResourceOperation.Update));
            if (order.OrderedDishes.IsNullOrEmpty())
            {
                throw new BadRequestException("You need to add dishes to order");
            }
            if (order.Status == Domain.Models.OrderStatus.InCart)
            {
                throw new BadRequestException("This order is not ordered yet");
            }
            if (order.Status == Domain.Models.OrderStatus.Realized)
            {
                throw new BadRequestException("This order is already finished");
            }
            order.Status = Domain.Models.OrderStatus.Realized;
            await _orderRepository.Commit();
        }
    }
}