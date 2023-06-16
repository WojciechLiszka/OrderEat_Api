using FluentValidation;

namespace OrderEat.Application.SpecialDiet.Commands
{
    public class DietDtoValidator : AbstractValidator<DietDto>
    {
        public DietDtoValidator()
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