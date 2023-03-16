using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Account.Command.UpdateUserDetails
{
    public class UpdateUserDetailsCommandValidator : AbstractValidator<UpdateUserDetailsCommand>
    {
        public UpdateUserDetailsCommandValidator()
        {
            RuleFor(x => x.Email)
                   .NotEmpty()
                   .NotNull()
                   .EmailAddress();

            RuleFor(x => x.ContactNumber)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(12);

            RuleFor(x => x.Country)
                .NotEmpty()
                .NotNull()
                .MaximumLength(30)
                .MinimumLength(2);

            RuleFor(x => x.City)
                .NotEmpty()
                .NotNull()
                .MaximumLength(30)
                .MinimumLength(2);

            RuleFor(x=>x.Street)
                .NotEmpty()
                .NotNull()
                .MaximumLength(30)
                .MinimumLength(2);

            RuleFor(x=>x.ApartmentNumber)
                .NotEmpty()
                .NotNull()
                .MaximumLength(10)
                .MinimumLength(1);

        }
    }
}