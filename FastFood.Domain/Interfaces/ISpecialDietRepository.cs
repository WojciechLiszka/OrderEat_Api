using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface ISpecialDietRepository
    {
        Task Create(SpecialDiet specialDiet);
    }
}