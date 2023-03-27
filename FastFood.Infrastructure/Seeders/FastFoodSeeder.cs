using FastFood.Domain.Entities;
using FastFood.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Seeders
{
    public class FastFoodSeeder
    {
        private readonly FastFoodDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEnumerable<Role> _roles;

        public FastFoodSeeder(FastFoodDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _roles = GetRoles();
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
                    _dbContext.Roles.AddRange(_roles);
                    await _dbContext.SaveChangesAsync();
                }
                if (!_dbContext.Users.Any())
                {
                    _dbContext.Users.Add(GetUser());
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

        private User GetUser()
        {
            var user = new User()
            {
                Email = "testadmin@test.com",
                Name = "adminUser",
                Role = _roles.FirstOrDefault(r => r.Name == "Admin")
            };

            var passwordhash = _passwordHasher.HashPassword(user, "TestAdminP@$$w0rd");

            user.PasswordHash = passwordhash;

            return user;
        }
    }
}