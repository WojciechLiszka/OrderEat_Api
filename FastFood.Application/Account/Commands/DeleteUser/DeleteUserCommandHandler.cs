using FastFood.Domain.Entities;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FastFood.Application.Account.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAccountRepository _repository;
        public DeleteUserCommandHandler(IAccountRepository repository, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmail(request.Email);
            if (user == null)
            {
                throw new BadRequestException("Invalid email or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }
            await _repository.Delete(user);
        }
    }
}