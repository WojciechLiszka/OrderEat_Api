using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Application.Dish.Queries;
using FastFood.Domain.Models;
using MediatR;

namespace FastFood.Application.Allergen.Queries.GetAllergens
{
    public class GetAllergensQuery : PagedResultDto,IRequest<PagedResult<AllergenDto>>
    {
        
    }
}