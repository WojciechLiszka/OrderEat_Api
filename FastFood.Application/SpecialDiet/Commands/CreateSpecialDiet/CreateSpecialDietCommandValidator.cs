using FluentValidation;

namespace FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommandValidator : AbstractValidator<CreateSpecialDietCommand>
    {
        public CreateSpecialDietCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(45);

            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(500);
        }
    }
}