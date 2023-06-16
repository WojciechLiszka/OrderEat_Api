using Domain.Domain.Exceptions;
using OrderEat.Application.Authorization;
using OrderEat.Domain.Interfaces;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderEat.Application.Order.Query.GetOrdersToRealizeFromRestaurant
{
    public class GetOrdersToRealizeFromRestaurantQueryHandler : IRequestHandler<GetOrdersToRealizeFromRestaurantQuery, PagedResult<Domain.Entities.Order>>
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public GetOrdersToRealizeFromRestaurantQueryHandler(IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        async Task<PagedResult<Domain.Entities.Order>> IRequestHandler<GetOrdersToRealizeFromRestaurantQuery, PagedResult<Domain.Entities.Order>>.Handle(GetOrdersToRealizeFromRestaurantQuery request, CancellationToken cancellationToken)
        {
            var restaurant = _restaurantRepository.GetById(request.RestaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not Found");
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new RestaurantResourceOperationRequirement(ResourceOperation.Read));
            var baseQuery = await _orderRepository.GetOrdersToRealizeFromRestaurant(restaurant.Id);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Domain.Entities.Order, object>>>
                {
                    { nameof(Domain.Entities.Order.OrderDate), b => b.OrderDate },
                };
                var selectedColumn = columnsSelectors[request.SortBy];

                baseQuery = request.SortDirection == SortDirection.ASC
                   ? baseQuery.OrderBy(selectedColumn)
                   : baseQuery.OrderByDescending(selectedColumn);
            }

            var orders = await baseQuery
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync();

            var totalItemsCount = baseQuery.Count();
            var result = new PagedResult<Domain.Entities.Order>(orders, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}