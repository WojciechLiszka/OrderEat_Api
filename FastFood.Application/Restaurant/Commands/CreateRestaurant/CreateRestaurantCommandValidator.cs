using FastFood.Domain.Interfaces;
using FluentValidation;

namespace FastFood.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        public CreateRestaurantCommandValidator(IRestaurantRepository repository)
        {
            RuleFor(m => m.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(m => m.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50)
                .Custom( (value, context) =>
                {
                    var existingRestaurant =  repository.GetByName(value).Result;
                    if (existingRestaurant != null)
                    {
                        context.AddFailure($"{value} is not unique name for restaurant");
                    }
                });

            RuleFor(m => m.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(2)
                .MaximumLength(500);

            RuleFor(m => m.ContactNumber)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(12);

            RuleFor(m => m.Country)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(m => m.City)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(m => m.Street)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);

            RuleFor(m => m.ApartmentNumber)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1)
                .MaximumLength(50);
        }
    }
}