using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Dish.Command.UpdateDish
{
    public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
    {
        private readonly IDishRepository _dishRepository;

        public UpdateDishCommandHandler(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.AllowedCustomization = request.AllowedCustomization;
            dish.BasePrize = request.BasePrize;
            dish.BaseCaloricValue = request.BaseCaloricValue;
            dish.IsAvilable = request.IsAvilable;

            await _dishRepository.Commit();
        }
    }
}