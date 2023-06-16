using OrderEat.Domain.Entities;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public RoleRepository(OrderEatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role?> GetById(int id)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return role;
        }

        public async Task<Role?> GetByName(string name)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
            return role;
        }
    }
}