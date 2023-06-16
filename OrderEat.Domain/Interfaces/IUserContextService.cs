using System.Security.Claims;

namespace OrderEat.Domain.Interfaces
{
    public interface IUserContextService
    {
        string? GetUserEmail { get; }
        int? GetUserId { get; }
        string? GetUserRole { get; }
        ClaimsPrincipal User { get; }
    }
}