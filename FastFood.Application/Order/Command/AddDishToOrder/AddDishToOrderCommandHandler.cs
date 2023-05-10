using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using FastFood.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Order.Command.AddDishToOrder
{
    public class AddDishToOrderCommandHandler : IRequestHandler<AddDishToOrderCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public AddDishToOrderCommandHandler(IIngredientRepository ingredientRepository, IOrderRepository orderRepository, IDishRepository dishRepository, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _ingredientRepository = ingredientRepository;
            _orderRepository = orderRepository;
            _dishRepository = dishRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task Handle(AddDishToOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(request.OrderId);

            if (order == null)
            {
                throw new NotFoundException("Order not Found");
            }

            var dish = await _dishRepository.GetByIdWithIngredients(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not Found");
            }
            if (dish.IsAvilable == false)
            {
                throw new ForbiddenException();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, order, new OrderRsourceOperationRequirement(ResourceOperation.Read));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }
            var orderedDish = new OrderedDish()
            {
                DishId = order.Id,
                Name = dish.Name,
                prize = dish.BasePrize
            };

            foreach (var ingredient in dish.AllowedIngreedients)
            {
                if (ingredient.IsRequired == true || request.AditionalIngrediens.IngredientsId.Contains(ingredient.Id))
                {
                    if (ingredient.IsRequired == false)
                    {
                        orderedDish.prize += ingredient.Prize;
                    }
                    orderedDish.Ingredients.Add(ingredient);
                }
            }

            order.OrderedDishes.Add(orderedDish);
            await _orderRepository.Commit();
        }
    }
}