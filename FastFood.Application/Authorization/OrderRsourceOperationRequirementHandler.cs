using FastFood.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FastFood.Application.Authorization
{
    public class OrderRsourceOperationRequirementHandler : AuthorizationHandler<OrderRsourceOperationRequirement, Domain.Entities.Order>
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public OrderRsourceOperationRequirementHandler(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderRsourceOperationRequirement requirement, Domain.Entities.Order resource)
        {
            if (requirement.ResourceOperation is ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            var restaurant = await _restaurantRepository.GetById(resource.RestaurantId);
            if (resource.UserId == userId || (userRole == "Owner" && restaurant.CreatedById == userId) || userRole == "Admin")
            {
                context.Succeed(requirement);
            };
        }
    }
}