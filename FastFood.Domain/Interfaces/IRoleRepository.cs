using FastFood.Domain.Entities;

namespace FastFood.Infrastructure.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetById(int id);
    }
}