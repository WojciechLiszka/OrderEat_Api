using FastFood.Domain.Entities;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Seeders
{
    public class FastFoodSeeder
    {
        private readonly FastFoodDbContext _dbContext;
        private readonly IEnumerable<Role> roles;

        public FastFoodSeeder(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
            roles = GetRoles();
        }

        public async Task Seed()
        {
            if (_dbContext.Database.CanConnect() && _dbContext.Database.IsRelational())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
                if (!_dbContext.Roles.Any())
                {
                    _dbContext.Roles.AddRange(roles);
                    await _dbContext.SaveChangesAsync();
                }
                
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>() {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                Name = "Owner"
                },
                new Role()
                {
                    Name = "Admin"
                }
                };

            return roles;
        }
    }
}