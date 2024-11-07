using BookRental.Domain.Entities;

namespace BookRental.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
