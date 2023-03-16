using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;

namespace FastFood.Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public DishRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}