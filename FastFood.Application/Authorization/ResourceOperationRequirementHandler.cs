using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FastFood.Application.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Domain.Entities.Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Domain.Entities.Restaurant resource)
        {
            if (requirement.ResourceOperation is ResourceOperation.Create or ResourceOperation.Read)
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            if (resource.CreatedById == userId || userRole == "Admin")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}