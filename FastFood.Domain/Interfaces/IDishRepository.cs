using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IDishRepository
    {
        Task Create(Domain.Entities.Dish dish);

        Task<Dish> GetById(int id);
    }
}