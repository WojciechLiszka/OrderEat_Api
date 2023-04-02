using FluentValidation;

namespace FastFood.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantDtoValidatior : AbstractValidator<UpdateRestaurantDto>
    {
        public UpdateRestaurantDtoValidatior()
        {
            RuleFor(m => m.Email)
               .NotEmpty()
               .NotNull()
               .EmailAddress();

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