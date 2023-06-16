using FluentValidation;

namespace OrderEat.Application.Account.Command.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                        .NotEmpty()
                        .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .NotEmpty()
                .NotNull();
        }
    }
}