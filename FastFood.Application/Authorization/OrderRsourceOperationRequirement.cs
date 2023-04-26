using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Authorization
{
    public class OrderRsourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation resourceOperation;

        public OrderRsourceOperationRequirement(ResourceOperation resourceOperation)
        {
            resourceOperation = resourceOperation;
        }

    }
}