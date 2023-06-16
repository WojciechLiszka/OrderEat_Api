using OrderEat.Domain.Entities;

namespace OrderEat.Infrastructure.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetById(int id);

        Task<Role?> GetByName(string name);
    }
}