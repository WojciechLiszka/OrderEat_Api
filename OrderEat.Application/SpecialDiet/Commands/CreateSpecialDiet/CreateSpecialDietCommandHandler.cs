using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.CreateSpecialDiet
{
    public class CreateSpecialDietCommandHandler : IRequestHandler<CreateSpecialDietCommand, string>
    {
        private readonly ISpecialDietRepository _repository;

        public CreateSpecialDietCommandHandler(ISpecialDietRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreateSpecialDietCommand request, CancellationToken cancellationToken)
        {
            var newDiet = new Domain.Entities.SpecialDiet()
            {
                Name = request.Name,
                Description = request.Description,
            };
            await _repository.Create(newDiet);
            return newDiet.Id.ToString();
        }
    }
}