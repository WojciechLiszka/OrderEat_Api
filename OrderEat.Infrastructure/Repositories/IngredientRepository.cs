using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public IngredientRepository(OrderEatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
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
            var ingredient = await _dbContext.Ingredients
                .Where(x => x.Id == Id)
                .Include(x => x.Dish)
                .FirstOrDefaultAsync();
            return ingredient;
        }
    }
}