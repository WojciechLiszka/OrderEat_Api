using OrderEat.Domain.Entities;
using OrderEat.Domain.Interfaces;
using OrderEat.Domain.Models;
using OrderEat.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace OrderEat.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderEatDbContext _dbContext;

        public OrderRepository(OrderEatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderedDishes)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<IQueryable<Order>> GetOrdersToRealizeFromRestaurant(int restaurantId)
        {
            var orders =  _dbContext
                 .Orders
                 .Where(c => c.RestaurantId == restaurantId && c.Status == OrderStatus.Ordered);

            return  orders;
        }
    }
}