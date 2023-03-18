using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;

namespace FastFood.Infrastructure.Repositories
{
    public class SpecialDietRepository : ISpecialDietRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public SpecialDietRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(SpecialDiet specialDiet)
        {
            _dbContext.Add(specialDiet);
            await _dbContext.SaveChangesAsync();
        }
    }
}