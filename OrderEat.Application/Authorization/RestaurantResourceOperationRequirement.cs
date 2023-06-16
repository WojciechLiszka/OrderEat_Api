using Microsoft.AspNetCore.Authorization;

namespace OrderEat.Application.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class RestaurantResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; set; }

        public RestaurantResourceOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}