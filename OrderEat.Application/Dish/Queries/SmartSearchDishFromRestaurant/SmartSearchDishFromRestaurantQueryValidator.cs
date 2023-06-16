using OrderEat.Application.Dish.Queries.SmartSearchDish;
using FluentValidation;

namespace OrderEat.Application.Dish.Queries.SmartSearchDishFromRestaurant
{
    public class SmartSearchDishFromRestaurantQueryValidator : AbstractValidator<SmartSearchDishFromRestaurantQuery>
    {
        private readonly int[] allowedPageSizes = new[] { 5, 10, 15 };

        private readonly string[] allowedSortByColumnNames =
            {nameof(Domain.Entities.Dish.Name), nameof(Domain.Entities.Dish.Description)};

        public SmartSearchDishFromRestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}