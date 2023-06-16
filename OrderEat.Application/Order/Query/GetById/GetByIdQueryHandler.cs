using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Order.Query.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Domain.Entities.Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public GetByIdQueryHandler(IOrderRepository orderRepository, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _orderRepository = orderRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public async Task<Domain.Entities.Order> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(request.Id);

            if (order == null)
            {
                throw new NotFoundException("Order Not Found");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, order, new OrderRsourceOperationRequirement(ResourceOperation.Read));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }
            return order;
        }
    }
}