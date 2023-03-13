using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IAllergenRepository
    {
        Task<int> Create(Allergen allergen);

        Task<Allergen?> GetById(int id);

        Task<Allergen?> GetByName(string name);

        Task Commit();

        Task Delete(Allergen allergen);
    }
}