using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task Create(Order order);

        Task<Order?> GetById(int id);
    }
}