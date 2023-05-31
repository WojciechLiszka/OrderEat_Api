using FastFood.Domain.Entities;
using FastFood.Domain.Models;

namespace FastFood.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task Create(Order order);

        Task<Order?> GetById(int id);

        Task Commit();

        Task<IQueryable<Order>> GetOrdersToRealizeFromRestaurant(int restaurantId, OrderStatus selectedStatus);
    }
}