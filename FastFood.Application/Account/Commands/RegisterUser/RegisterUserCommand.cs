using MediatR;

namespace FastFood.Application.Account.Command.RegisterUser
{
    public class RegisterUserCommand : IRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string Name { get; set; }
    }
}