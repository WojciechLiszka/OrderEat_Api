using OrderEat.Application.Allergen.Commands.CreateAllergen;
using MediatR;

namespace OrderEat.Application.Allergen.Commands.UpdateAllergen
{
    public class UpdateAllergenCommand : AllergenDto, IRequest
    {
        public int Id { get; set; }
    }
}