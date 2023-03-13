using FastFood.Application.Allergen.Commands.CreateAllergen;
using MediatR;

namespace FastFood.Application.Allergen.Commands.UpdateAllergen
{
    public class UpdateAllergenCommand : AllergenDto, IRequest
    {
        public int Id { get; set; }
    }
}