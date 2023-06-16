using AutoMapper;
using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderEat.Application.Dish.Queries.GedDishesFromRestaurant
{
    public class GetDishesFromRestaurantQueryHandler : IRequestHandler<GetDishesFromRestaurantQuery, PagedResult<GetDishDto>>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetDishesFromRestaurantQueryHandler(IDishRepository dishRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetDishDto>> Handle(GetDishesFromRestaurantQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _restaurantRepository.GetById(request.RestaurantId);
            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            var baseQuery = _dishRepository.Search(restaurant.Id, request.SearchPhrase);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Domain.Entities.Dish, object>>>
                {
                    { nameof(Domain.Entities.Dish.Name), b => b.Name },
                    { nameof(Domain.Entities.Dish.Description), b => b.Description },
                };
                var selectedColumn = columnsSelectors[request.SortBy];

                baseQuery = request.SortDirection == SortDirection.ASC
                   ? baseQuery.OrderBy(selectedColumn)
                   : baseQuery.OrderByDescending(selectedColumn);
            }

            var dishes = await baseQuery
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync();

            var totalItemsCount = baseQuery.Count();
            var dtos = _mapper.Map<List<GetDishDto>>(dishes);

            var result = new PagedResult<GetDishDto>(dtos, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}