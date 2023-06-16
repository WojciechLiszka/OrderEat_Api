using OrderEat.Domain.Interfaces;
using FluentValidation;

namespace OrderEat.Application.Allergen.Commands.CreateAllergen
{
    public class CreateAllergenCommandValidator : AbstractValidator<CreateAllergenCommand>
    {
        public CreateAllergenCommandValidator(IAllergenRepository repository)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(45)
                .Custom(async (value, context) =>
                {
                    var existingAllergen =await repository.GetByName(value);
                    if (existingAllergen != null)
                    {
                        context.AddFailure($"{value} is not unique name for allergen");
                    }
                });

            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(500);
        }
    }
}