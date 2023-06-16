using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommand : DietDto, IRequest<string>
    {
    }
}