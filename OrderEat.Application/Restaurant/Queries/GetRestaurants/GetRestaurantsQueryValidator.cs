using FluentValidation;

namespace OrderEat.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQueryValidator : AbstractValidator<GetRestaurantsQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
            {nameof(Domain.Entities.Restaurant.Name), nameof(Domain.Entities.Restaurant.Description)};

        public GetRestaurantsQueryValidator()
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