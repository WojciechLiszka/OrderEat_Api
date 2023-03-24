using Domain.Domain.Exceptions;
using FastFood.Domain.Entities;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;
using MediatR;

namespace FastFood.Application.Account.Command.UpdateUserDetails
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IUserContextService _userContextService;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserDetailsCommandHandler(IAccountRepository repository, IUserContextService userContextService, IRoleRepository roleRepository)
        {
            _repository = repository;
            _userContextService = userContextService;
            _roleRepository = roleRepository;
        }

        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var userEmail = _userContextService.GetUserEmail;
            if (userEmail == null)
            {
                throw new BadRequestException("Invalid user Token");
            }

            var user = await _repository.GetByEmail(userEmail);
            if (user == null)
            {
                throw new NotFoundException("Invalid user Token");
            }

            user.ContactDetails = new UserContactDetails()
            {
                Country = request.Country,
                City = request.City,
                Street = request.Street,
                ApartmentNumber = request.ApartmentNumber,
                ContactNumber = request.ContactNumber
            };

            user.DateofBirth = request.DateofBirth;
            user.Name = request.Name;

            if (user.Role == null)
            {
                var userRole = await _roleRepository.GetByName("User");
                user.Role = userRole;
            }
            await _repository.Commit();
        }
    }
}