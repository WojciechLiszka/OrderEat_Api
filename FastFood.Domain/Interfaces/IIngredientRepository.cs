using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task Create(Ingredient ingredient);

        Task<Ingredient?> GetById(int Id);
    }
}