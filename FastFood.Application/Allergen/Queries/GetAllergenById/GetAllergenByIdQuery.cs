using FastFood.Application.Allergen.Commands.CreateAllergen;
using MediatR;

namespace FastFood.Application.Allergen.Queries.GetAllergenById
{
    public class GetAllergenByIdQuery : IRequest<AllergenDto>
    {
        public int Id { get; set; }
    }
}