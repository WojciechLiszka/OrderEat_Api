using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task Commit();

        Task Create(Ingredient ingredient);

        Task Delete(Ingredient ingredient);

        Task<Ingredient?> GetById(int Id);
    }
}