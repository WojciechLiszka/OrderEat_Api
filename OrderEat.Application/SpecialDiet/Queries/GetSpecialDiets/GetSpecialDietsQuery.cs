using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.SpecialDiet.Queries.GetSpecialDiets
{
    public class GetSpecialDietsQuery : PagedResultDto, IRequest<PagedResult<GetDietDto>>
    {

    }
}