using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.UpdateSpecialDiet
{
    public class UpdateSpecialDietCommandHandler : IRequestHandler<UpdateSpecialDietCommand>
    {
        private readonly ISpecialDietRepository _repository;

        public UpdateSpecialDietCommandHandler(ISpecialDietRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateSpecialDietCommand request, CancellationToken cancellationToken)
        {
            var diet = await _repository.GetById(request.Id);

            if (diet == null) 
            {
                throw new NotFoundException("Diet not found");
            }
            diet.Name = request.Name;
            diet.Description = request.Description;
            await _repository.Commit();
        }
    }
}