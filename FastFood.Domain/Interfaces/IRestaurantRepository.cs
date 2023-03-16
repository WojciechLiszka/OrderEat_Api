using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IRestaurantRepository
    {
        Task Commit();
        Task Create(Restaurant restaurant);

        Task<Restaurant?> GetByName(string name);

        Task<Restaurant?> GetById(int id);

        IQueryable<Restaurant> Search(string? phrase);

        Task Delete(Restaurant restaurant);
    }
}