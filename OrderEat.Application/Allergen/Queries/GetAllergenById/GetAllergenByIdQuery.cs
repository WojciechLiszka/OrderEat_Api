using OrderEat.Application.Allergen.Commands.CreateAllergen;
using MediatR;

namespace OrderEat.Application.Allergen.Queries.GetAllergenById
{
    public class GetAllergenByIdQuery : IRequest<AllergenDto>
    {
        public int Id { get; set; }
    }
}