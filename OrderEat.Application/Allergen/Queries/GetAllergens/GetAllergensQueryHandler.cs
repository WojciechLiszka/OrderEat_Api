using AutoMapper;
using OrderEat.Application.Allergen.Commands.CreateAllergen;
using OrderEat.Domain.Interfaces;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OrderEat.Application.Allergen.Queries.GetAllergens
{
    public class GetAllergensQueryHandler : IRequestHandler<GetAllergensQuery, PagedResult<AllergenDto>>
    {
        private readonly IAllergenRepository _repository;
        private readonly IMapper _mapper;

        public GetAllergensQueryHandler(IAllergenRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<AllergenDto>> Handle(GetAllergensQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _repository.Search(request.SearchPhrase);
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Domain.Entities.Allergen, object>>>
                {
                    { nameof(Domain.Entities.Allergen.Name), b => b.Name },
                    { nameof(Domain.Entities.Allergen.Description), b => b.Description },
                };
                var selectedColumn = columnsSelectors[request.SortBy];

                baseQuery = request.SortDirection == SortDirection.ASC
                   ? baseQuery.OrderBy(selectedColumn)
                   : baseQuery.OrderByDescending(selectedColumn);
            }
            var allergens = await baseQuery
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync();
            var totalItemsCount = baseQuery.Count();
            var dtos = _mapper.Map<List<AllergenDto>>(allergens);
            var result = new PagedResult<AllergenDto>(dtos, totalItemsCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}