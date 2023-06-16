using Domain.Domain.Exceptions;
using OrderEat.Domain.Entities;
using OrderEat.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.Account.Command.UpdateUserDetails
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IUserContextService _userContextService;

        public UpdateUserDetailsCommandHandler(IAccountRepository repository, IUserContextService userContextService)
        {
            _repository = repository;
            _userContextService = userContextService;
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

            await _repository.Commit();
        }
    }
}