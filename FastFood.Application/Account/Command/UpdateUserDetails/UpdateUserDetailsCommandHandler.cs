using Domain.Domain.Exceptions;
using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Account.Command.UpdateUserDetails
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
    {
        private readonly IAccountRepository _repository;

        public UpdateUserDetailsCommandHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmail(request.Email);
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
                // ToDo user.Role==new RoleUser
            }
        }
    }
}