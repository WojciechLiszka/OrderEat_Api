using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface ISpecialDietRepository
    {
        Task Commit();

        Task Create(SpecialDiet specialDiet);

        Task<SpecialDiet?> GetById(int id);

        Task Delete(SpecialDiet specialDiet);

        IQueryable<SpecialDiet> Search(string? phrase);
    }
}