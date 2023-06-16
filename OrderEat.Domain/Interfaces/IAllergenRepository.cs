using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface IAllergenRepository
    {
        Task Commit();

        Task<int> Create(Allergen allergen);

        Task Delete(Allergen allergen);

        Task<Allergen?> GetById(int id);

        Task<Allergen?> GetByName(string name);

        IQueryable<Allergen> Search(string? phrase);
    }
}