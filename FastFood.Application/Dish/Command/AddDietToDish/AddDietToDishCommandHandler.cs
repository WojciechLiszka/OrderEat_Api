using Domain.Domain.Exceptions;
using FastFood.Application.Authorization;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Application.Dish.Command.AddDietToDish
{
    public class AddDietToDishCommandHandler : IRequestHandler<AddDietToDishCommand>
    {
        private readonly IDishRepository _dishRepository;
        private readonly ISpecialDietRepository _specialDietRepository;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContext;

        public AddDietToDishCommandHandler(IDishRepository dishRepository, ISpecialDietRepository specialDietRepository, IAuthorizationService authorization, IUserContextService userContext)
        {
            _dishRepository = dishRepository;
            _specialDietRepository = specialDietRepository;
            _authorization = authorization;
            _userContext = userContext;
        }

        public async Task Handle(AddDietToDishCommand request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetByIdWithAllowedDiets(request.DishId);
            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }
            var authorizationResult = await _authorization.AuthorizeAsync(_userContext.User, dish.Restaurant, new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
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