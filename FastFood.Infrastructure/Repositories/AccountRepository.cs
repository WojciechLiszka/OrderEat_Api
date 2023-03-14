using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;

namespace FastFood.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public AccountRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}