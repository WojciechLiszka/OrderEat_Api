using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Dish.Command.AddDietToDish
{
    public class AddDietToDishCommandHandler : IRequestHandler<AddDietToDishCommand>
    {
        private readonly IDishRepository _dishRepository;
        private readonly ISpecialDietRepository _specialDietRepository;

        public AddDietToDishCommandHandler(IDishRepository dishRepository, ISpecialDietRepository specialDietRepository)
        {
            _dishRepository = dishRepository;
            _specialDietRepository = specialDietRepository;
        }

        public async Task Handle(AddDietToDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetByIdWithAllowedDiets(request.DishId);
            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var diet = await _specialDietRepository.GetById(request.DietId);

            if (diet == null)
            {
                throw new NotFoundException("Diet not found");
            }

            dish.AllowedForDiets.Add(diet);

            await _dishRepository.Commit();
        }
    }
}