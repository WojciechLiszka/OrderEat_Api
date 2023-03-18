using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IDishRepository
    {
        Task Commit();

        Task Create(Dish dish);
        
        Task Delete(Dish dish);

        Task<Dish> GetById(int id);

        Task<Dish> GetByIdWithIngredients(int id);

        IQueryable<Dish> Search(int id,string? phrase);
    }
}