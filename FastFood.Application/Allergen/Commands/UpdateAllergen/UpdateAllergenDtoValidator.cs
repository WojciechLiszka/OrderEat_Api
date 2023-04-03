using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Allergen.Commands.UpdateAllergen
{
    public class UpdateAllergenDtoValidator:AbstractValidator<UpdateAllergenDto>
    {
        public UpdateAllergenDtoValidator(IAllergenRepository repository)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(45)
                .Custom(async (value, context) =>
                {
                    var existingAllergen = await repository.GetByName(value);
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