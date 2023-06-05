using AutoMapper;
using Domain.Domain.Exceptions;
using FastFood.Application.Dish.Queries.SmartSearchDish;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastFood.Application.Dish.Queries.SmartSearchDishFromRestaurant
{
    public class SmartSearchDishFromRestaurantQueryHandler : IRequestHandler<SmartSearchDishFromRestaurantQuery, PagedResult<GetDishDto>>
    {
        private readonly IUserContextService _userContextService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SmartSearchDishFromRestaurantQueryHandler(IUserContextService userContextService,IRestaurantRepository restaurantRepository,IDishRepository dishRepository,IUserRepository userRepository,IMapper mapper)
        {
            _userContextService = userContextService;
            _restaurantRepository = restaurantRepository;
            _dishRepository = dishRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<PagedResult<GetDishDto>> Handle(SmartSearchDishFromRestaurantQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId;
            if(userId == null) 
            {
                throw new BadRequestException("Invalid user token");
            }
            var user=await _userRepository.GetUserById((int)userId);
            if (user == null) 
            {
                throw new BadRequestException("Invalid user token");
            }
            var diet = user.Diet;
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
            var list= (List<Domain.Entities.Dish>)baseQuery;
            
            if (diet != null)
            {
                foreach(var dish in  list) 
                {
                    if (dish.AllowedForDiets.Contains(diet)==false)
                    {
                       list.Remove(dish);
                    }
                }
            }

            var dishes =  list
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToList();
            

            var totalItemsCount = baseQuery.Count();
            var dtos = _mapper.Map<List<GetDishDto>>(dishes);

            var result = new PagedResult<GetDishDto>(dtos, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}