using MediatR;

namespace OrderEat.Application.Allergen.Commands.CreateAllergen
{
    public class CreateAllergenCommand : AllergenDto, IRequest<int>
    {
    }
}