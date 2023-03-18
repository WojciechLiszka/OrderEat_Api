using MediatR;

namespace FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommand : DietDto, IRequest<string>
    {
    }
}