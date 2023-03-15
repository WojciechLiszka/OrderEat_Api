using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IAccountRepository
    {
        bool EmailInUse(string email);

        Task Register(User user);

        Task<User?> GetByEmail(string email);
    }
}