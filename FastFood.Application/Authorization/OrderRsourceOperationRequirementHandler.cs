using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FastFood.Application.Authorization
{
    public class OrderRsourceOperationRequirementHandler : AuthorizationHandler<OrderRsourceOperationRequirement, Domain.Entities.Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderRsourceOperationRequirement requirement, Domain.Entities.Order resource)
        {
            if (requirement.resourceOperation is ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            if (resource.UserId == userId || userRole == "Admin")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}