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

        public async Task Create(Dish dish)
        {
            _dbContext.Add(dish);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Dish> GetById(int id)
        {
            var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            return dish;
        }
    }
}