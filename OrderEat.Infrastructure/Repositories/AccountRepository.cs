using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public AccountRepository(OrderEatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public bool EmailInUse(string email)
            => _dbContext.Users.Any(u => u.Email == email);

        public async Task<User?> GetByEmail(string email)
            => await _dbContext
            .Users
            .Include(u => u.ContactDetails)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        public async Task Register(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return;
        }
    }
}