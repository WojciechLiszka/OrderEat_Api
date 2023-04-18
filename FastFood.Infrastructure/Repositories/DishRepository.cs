using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public DishRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(Dish dish)
        {
            _dbContext.Add(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Dish dish)
        {
            _dbContext.Remove(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dish?> GetById(int id)
        {
            var dish = await _dbContext.Dishes
                .FirstOrDefaultAsync(d => d.Id == id);
            return dish;
        }

        public async Task<Dish?> GetByIdWithAllowedDiets(int id)
        {
            var dish = await _dbContext.Dishes
               .Include(x => x.AllowedForDiets)
               .FirstOrDefaultAsync(d => d.Id == id);
            return dish;
        }

        public async Task<Dish?> GetByIdWithIngredients(int id)
        {
            var dish = await _dbContext.Dishes
                .Include(x => x.BaseIngreedients)
                .FirstOrDefaultAsync(d => d.Id == id);

            return dish;
        }

        public IQueryable<Dish> Search(int id, string? phrase)
        {
            var dishes = _dbContext.Dishes.Where(d => d.RestaurantId == id
            && (phrase == null
            || d.Name.ToLower().Contains(phrase.ToLower())
            || d.Description.ToLower().Contains(phrase.ToLower())));
            return dishes;
        }
    }
}