using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Allergen
{
    public class AllergenDtoValidator : AbstractValidator<AllergenDto>
    {
        public AllergenDtoValidator(IAllergenRepository repository)
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