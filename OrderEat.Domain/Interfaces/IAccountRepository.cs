using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task Commit();

        Task Delete(User user);

        bool EmailInUse(string email);

        Task<User?> GetByEmail(string email);

        Task Register(User user);
    }
}