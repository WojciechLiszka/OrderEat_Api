using MediatR;

namespace FastFood.Application.Allergen.Commands.CreateAllergen
{
    public class CreateAllergenCommand : AllergenDto, IRequest
    {
    }
}