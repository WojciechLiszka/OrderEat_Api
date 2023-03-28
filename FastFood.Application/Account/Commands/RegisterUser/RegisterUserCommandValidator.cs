using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Account.Command.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(IAccountRepository repository)
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .Length(5, 254);

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
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 20);
        }
    }
}