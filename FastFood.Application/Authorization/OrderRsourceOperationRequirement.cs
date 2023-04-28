using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Authorization
{
    public class OrderRsourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; set; }

        public OrderRsourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}