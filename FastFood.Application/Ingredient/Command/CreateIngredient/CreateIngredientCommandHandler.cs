using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Ingredient.Command.CreateIngredient
{
    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, string>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public CreateIngredientCommandHandler(IDishRepository dishRepository, IIngredientRepository ingredientRepository)
        {
            _dishRepository = dishRepository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<string> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var newIngredient = new Domain.Entities.Ingredient()
            {
                Name = request.Name,
                Description = request.Description,
                IsRequired = request.IsRequired,
                Prize = request.Prize,
                Dish = dish
            };

            await _ingredientRepository.Create(newIngredient);

            return newIngredient.Id.ToString();
        }
    }
}