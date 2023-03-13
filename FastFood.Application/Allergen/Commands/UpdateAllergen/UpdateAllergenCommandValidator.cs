using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Allergen.Commands.UpdateAllergen
{
    public class UpdateAllergenCommandValidator : AbstractValidator<UpdateAllergenCommand>
    {
        public UpdateAllergenCommandValidator(IAllergenRepository repository)
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(45)
                .Custom((value, context) =>
                {
                    var existingAllergen = repository.GetByName(value).Result;
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