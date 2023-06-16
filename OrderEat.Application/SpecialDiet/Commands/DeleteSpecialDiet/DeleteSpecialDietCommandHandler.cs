using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.SpecialDiet.Commands.DeleteSpecialDiet
{
    public class DeleteSpecialDietCommandHandler : IRequestHandler<DeleteSpecialDietCommand>
    {
        private readonly ISpecialDietRepository _repository;

        public DeleteSpecialDietCommandHandler(ISpecialDietRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteSpecialDietCommand request, CancellationToken cancellationToken)
        {
            var diet = await _repository.GetById(request.Id);
            if (diet == null)
            {
                throw new NotFoundException("Diet not found");
            }
            await _repository.Delete(diet);
        }
    }
}