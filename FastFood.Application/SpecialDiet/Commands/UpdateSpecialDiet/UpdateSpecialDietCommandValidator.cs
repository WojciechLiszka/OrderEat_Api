using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.SpecialDiet.Commands.UpdateSpecialDiet
{
    public class UpdateSpecialDietCommandValidator : AbstractValidator<UpdateSpecialDietCommand>
    {
        public UpdateSpecialDietCommandValidator(ISpecialDietRepository repository)
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(45)
                 .Custom(async (value, context) =>
                 {
                     var existingAllergen = await repository.GetByName(value);
                     if (existingAllergen != null)
                     {
                         context.AddFailure($"{value} is not unique name for Diet");
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