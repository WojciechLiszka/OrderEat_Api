using MediatR;

namespace FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommand : IRequest<string>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}