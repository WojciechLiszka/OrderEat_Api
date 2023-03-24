﻿using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;
using MediatR;

namespace FastFood.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, string>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IUserContextService _userContextService;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;

        public CreateRestaurantCommandHandler(IRestaurantRepository repository, IUserContextService userContextService, IRoleRepository roleRepository, IAccountRepository accountRepository)
        {
            _repository = repository;
            _userContextService = userContextService;
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
        }

        public async Task<string> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetUserId;
            if (userId is null)
            {
                throw new BadRequestException("Invalid user token");
            }

            var newRestaurant = new Domain.Entities.Restaurant()
            {
                Name = request.Name,
                Description = request.Description,
                CreatedById = (int)userId,
                ContactDetails = new Domain.Entities.RestaurantContactDetails()
                {
                    ContactNumber = request.ContactNumber,
                    Email = request.Email,
                    Country = request.Country,
                    City = request.City,
                    Street = request.Street,
                    ApartmentNumber = request.ApartmentNumber
                }
            };

            await _repository.Create(newRestaurant);
            var useRole = _userContextService.GetUserRole;

            if (useRole == "User" || useRole == null)
            {
                var user = await _accountRepository.GetByEmail(_userContextService.GetUserEmail);
                var ownerRole = await _roleRepository.GetByName("Owner");
                user.Role = ownerRole;
                await _accountRepository.Commit();
            }

            return newRestaurant.Id.ToString();
        }
    }
}