using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public UserRepository(OrderEatDbContext dbContext)
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