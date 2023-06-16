using FluentValidation;

namespace OrderEat.Application.SpecialDiet.Queries.GetSpecialDiets
{
    public class GetSpecialDietsQueryValidator : AbstractValidator<GetSpecialDietsQuery>
    {
        private readonly int[] allowedPageSizes = new[] { 5, 10, 15 };

        private readonly string[] allowedSortByColumnNames =
            {nameof(Domain.Entities.SpecialDiet.Name), nameof(Domain.Entities.SpecialDiet.Description)};
        public GetSpecialDietsQueryValidator()
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