﻿using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Dish.Command.UpdateDish
{
    public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContext;

        public UpdateDishCommandHandler(IDishRepository dishRepository, IAuthorizationService authorizationService, IUserContextService userContext)
        {
            _dishRepository = dishRepository;
            _authorizationService = authorizationService;
            _userContext = userContext;
        }

        public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContext.User, dish.Restaurant, new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
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