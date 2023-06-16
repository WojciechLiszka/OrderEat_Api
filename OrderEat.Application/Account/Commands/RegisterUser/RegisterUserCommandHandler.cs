using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace OrderEat.Application.Account.Command.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRoleRepository _roleRepository;

        public RegisterUserCommandHandler(IAccountRepository repository, IPasswordHasher<User> passwordHasher, IRoleRepository roleRepository)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _roleRepository.GetByName("Owner");
            var newUser = new User()
            {
                Email = request.Email,
                Name = request.Name,
                Role = userRole
            };

            var passwordhash = _passwordHasher.HashPassword(newUser, request.Password);

            newUser.PasswordHash = passwordhash;

            await _repository.Register(newUser);
        }
    }
}