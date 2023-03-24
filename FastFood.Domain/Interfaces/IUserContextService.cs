using System.Security.Claims;

namespace FastFood.Domain.Interfaces
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
        string? GetUserRole { get; }
        string? GetUserEmail { get; }
    }
}