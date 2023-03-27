using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommandValidator : AbstractValidator<CreateSpecialDietCommand>
    {
        public CreateSpecialDietCommandValidator(ISpecialDietRepository repository)
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(45)
                .Custom((value, context) =>
                {
                    var existingAllergen = repository.GetByName(value).Result;
                    if (existingAllergen != null)
                    {
                        context.AddFailure($"{value} is not unique name for Diet");
                    }
                }); ;

            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(500);
        }
    }
}