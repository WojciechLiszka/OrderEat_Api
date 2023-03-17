using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

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

        public async Task Delete(Ingredient ingredient)
        {
            _dbContext.Remove(ingredient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Ingredient?> GetById(int Id)
        {
            var ingredient = await _dbContext.Ingredients.FirstOrDefaultAsync(x => x.Id == Id);
            return ingredient;
        }
    }
}