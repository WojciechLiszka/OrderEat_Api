using FastFood.Domain.Entities;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public RoleRepository(FastFoodDbContext dbContext)
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