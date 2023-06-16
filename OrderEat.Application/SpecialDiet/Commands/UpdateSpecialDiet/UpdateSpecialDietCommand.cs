using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.UpdateSpecialDiet
{
    public class UpdateSpecialDietCommand : DietDto, IRequest
    {
        public int Id { get; set; }
    }
}