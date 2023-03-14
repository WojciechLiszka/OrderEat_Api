using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Account.Command
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(IAccountRepository repository)
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                    .Custom((value, context) =>
                    {
                        var emailInUse = repository.EmailInUse(value);
                        if (emailInUse)
                        {
                            context.AddFailure("Email", "That email is taken");
                        }
                    });
        }
    }
}