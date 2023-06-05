using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public UserRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _dbContext
                .Users
                .Include(u => u.Diet)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}