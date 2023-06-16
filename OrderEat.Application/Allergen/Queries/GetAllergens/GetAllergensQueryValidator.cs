using FluentValidation;

namespace OrderEat.Application.Allergen.Queries.GetAllergens
{
    public class GetAllergensQueryValidator : AbstractValidator<GetAllergensQuery>
    {
        private readonly int[] allowedPageSizes = new[] { 5, 10, 15 };

        private readonly string[] allowedSortByColumnNames =
            {nameof(Domain.Entities.Allergen.Name), nameof(Domain.Entities.Allergen.Description)};

        public GetAllergensQueryValidator()
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