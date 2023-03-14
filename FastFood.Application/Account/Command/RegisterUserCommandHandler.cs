using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FastFood.Application.Account.Command
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IAccountRepository repository, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User()
            {
                Email = request.Email
            };

            var passwordhash = _passwordHasher.HashPassword(newUser, request.Password);

            newUser.PasswordHash = passwordhash;

            await _repository.Register(newUser);
        }
    }
}