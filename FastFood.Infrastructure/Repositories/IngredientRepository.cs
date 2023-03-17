using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;

namespace FastFood.Infrastructure.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public IngredientRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Ingredient ingredient)
        {
            _dbContext.Add(ingredient);
            await _dbContext.SaveChangesAsync();
        }
    }
}