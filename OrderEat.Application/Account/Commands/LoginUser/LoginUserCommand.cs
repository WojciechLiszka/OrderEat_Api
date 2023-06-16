using MediatR;

namespace OrderEat.Application.Account.Command
{
    public class LoginUserCommand : IRequest<string>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}