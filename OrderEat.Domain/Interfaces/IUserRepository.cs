using OrderEat.Domain.Entities;

namespace OrderEat.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
    }
}