using MediatR;

namespace FastFood.Application.Account.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommand : IRequest
    {
        public string UserEmail { get; set; }
        public int RoleId { get; set; }
    }
}