using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQuery : IRequest<PagedResult<GetRestaurantDto>>
    {
        public string? SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}