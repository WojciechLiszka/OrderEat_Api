using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface ISpecialDietRepository
    {
        Task Commit();

        Task Create(SpecialDiet specialDiet);

        Task Delete(SpecialDiet specialDiet);

        Task<SpecialDiet?> GetById(int id);

        Task<SpecialDiet?> GetByName(string name);

        IQueryable<SpecialDiet> Search(string? phrase);
    }
}