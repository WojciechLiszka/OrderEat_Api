using AutoMapper;
using OrderEat.Domain.Interfaces;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderEat.Application.Restaurant.Queries.GetRestaurants
{
    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, PagedResult<GetRestaurantDto>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper;

        public GetRestaurantsQueryHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetRestaurantDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _repository.Search(request.SearchPhrase);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Domain.Entities.Restaurant, object>>>
                {
                    { nameof(Domain.Entities.Restaurant.Name), b => b.Name },
                    { nameof(Domain.Entities.Restaurant.Description), b => b.Description },
                };
                var selectedColumn = columnsSelectors[request.SortBy];

                baseQuery = request.SortDirection == SortDirection.ASC
                   ? baseQuery.OrderBy(selectedColumn)
                   : baseQuery.OrderByDescending(selectedColumn);
            }
            var restaurants = await baseQuery
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync();
            var totalItemsCount = baseQuery.Count();
            var dtos = _mapper.Map<List<GetRestaurantDto>>(restaurants);
            var result = new PagedResult<GetRestaurantDto>(dtos, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}