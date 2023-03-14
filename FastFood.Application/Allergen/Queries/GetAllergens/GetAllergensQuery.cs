using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Allergen.Queries.GetAllergens
{
    public class GetAllergensQuery : IRequest<PagedResult<AllergenDto>>
    {
        public string? SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}