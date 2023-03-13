using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IAllergenRepository
    {
        Task Create(Allergen allergen);

        Task<Allergen?> GetById(int id);

        Task<Allergen?> GetByName(string name);
    }
}