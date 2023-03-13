using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IAllergenRepository
    {
        Task Create(Allergen allergen);
    }
}