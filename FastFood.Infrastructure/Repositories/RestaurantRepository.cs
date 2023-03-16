using FastFood.Domain.Entities;
using FastFood.Domain.Interfaces;
using FastFood.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly FastFoodDbContext _dbContext;

        public RestaurantRepository(FastFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Restaurant restaurant)
        {
            _dbContext.Restaurants.Add(restaurant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Restaurant?> GetById(int id)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }

        public async Task<Restaurant?> GetByName(string name)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
            return restaurant;
        }

        public IQueryable<Restaurant> Search(string? phrase)
        {
            var restaurants = _dbContext
               .Restaurants
               .Where(b => phrase == null
               || b.Name.ToLower().Contains(phrase.ToLower())
               || b.Description.ToLower().Contains(phrase.ToLower()));
            return restaurants;
        }
    }
}