using AutoMapper;
using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastFood.Application.SpecialDiet.Queries.GetSpecialDiets
{
    public class GetSpecialDietsQueryHandler : IRequestHandler<GetSpecialDietsQuery, PagedResult<GetDietDto>>
    {
        private readonly ISpecialDietRepository _specialDietRepository;
        private readonly IMapper _mapper;

        public GetSpecialDietsQueryHandler(ISpecialDietRepository specialDietRepository,IMapper mapper)
        {
            _specialDietRepository = specialDietRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<GetDietDto>> Handle(GetSpecialDietsQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _specialDietRepository.Search(request.SearchPhrase);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Domain.Entities.SpecialDiet, object>>>
                {
                    { nameof(Domain.Entities.SpecialDiet.Name), b => b.Name },
                    { nameof(Domain.Entities.SpecialDiet.Description), b => b.Description },
                };
                var selectedColumn = columnsSelectors[request.SortBy];

                baseQuery = request.SortDirection == SortDirection.ASC
                   ? baseQuery.OrderBy(selectedColumn)
                   : baseQuery.OrderByDescending(selectedColumn);
            }
            var diets = await baseQuery
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync();
            var totalItemsCount = baseQuery.Count();
            var dtos = _mapper.Map<List<GetDietDto>>(diets);
            var result = new PagedResult<GetDietDto>(dtos, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}