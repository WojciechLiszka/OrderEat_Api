using MediatR;

namespace OrderEat.Application.Account.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public string Email { get; set; } = default!;
        public string? Password { get; set; }
    }
}