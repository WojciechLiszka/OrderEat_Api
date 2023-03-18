using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.SpecialDiet.Queries.GetSpecialDiets
{
    public class GetSpecialDietsQuery : PagedResultDto, IRequest<PagedResult<GetDietDto>>
    {

    }
}