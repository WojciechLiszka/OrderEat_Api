using OrderEat.Application.Allergen.Commands.CreateAllergen;
using OrderEat.Application.Dish.Queries;
using OrderEat.Domain.Models;
using MediatR;

namespace OrderEat.Application.Allergen.Queries.GetAllergens
{
    public class GetAllergensQuery : PagedResultDto,IRequest<PagedResult<AllergenDto>>
    {
        
    }
}