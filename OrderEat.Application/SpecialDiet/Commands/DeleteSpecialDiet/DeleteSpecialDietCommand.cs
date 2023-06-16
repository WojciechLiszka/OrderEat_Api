using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.DeleteSpecialDiet
{
    public class DeleteSpecialDietCommand :IRequest
    {
        public int Id { get; set; }
    }
}