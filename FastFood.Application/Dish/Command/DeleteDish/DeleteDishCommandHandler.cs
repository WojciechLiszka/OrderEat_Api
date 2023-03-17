using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Dish.Command.DeleteDish
{
    public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand>
    {
        private readonly IDishRepository _repository;

        public DeleteDishCommandHandler(IDishRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _repository.GetById(request.id);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            await _repository.Delete(dish);
        }
    }
}