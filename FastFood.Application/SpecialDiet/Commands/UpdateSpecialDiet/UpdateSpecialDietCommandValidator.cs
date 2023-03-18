using FluentValidation;

namespace FastFood.Application.SpecialDiet.Commands.UpdateSpecialDiet
{
    public class UpdateSpecialDietCommandValidator : AbstractValidator<UpdateSpecialDietCommand>
    {
        public UpdateSpecialDietCommandValidator()
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