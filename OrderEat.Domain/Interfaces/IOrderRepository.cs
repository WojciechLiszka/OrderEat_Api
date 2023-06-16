using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task Create(Order order);

        Task<Order?> GetById(int id);

        Task Commit();

        Task<IQueryable<Order>> GetOrdersToRealizeFromRestaurant(int restaurantId);
    }
}