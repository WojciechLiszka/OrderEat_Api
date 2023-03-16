using MediatR;

namespace FastFood.Application.Account.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public string Email { get; set; } = default!;
        public string? Password { get; set; }
    }
}