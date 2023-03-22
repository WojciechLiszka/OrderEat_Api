using Domain.Domain.Exceptions;
using FastFood.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Repositories;
using MediatR;

namespace FastFood.Application.Account.Commands.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserRoleCommandHandler(IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetByEmail(request.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var role = await _roleRepository.GetById(request.RoleId);
            if (role == null)
            {
                throw new NotFoundException("Role not found");
            }
            if (user.Role.Id == role.Id)
            {
                throw new BadRequestException($"User is already in Role: {role.Name}");
            }
            user.Role = role;
        }
    }
}