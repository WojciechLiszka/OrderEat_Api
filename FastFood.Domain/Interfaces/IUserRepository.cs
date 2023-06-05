using FastFood.Domain.Entities;

namespace FastFood.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
    }
}