using BookRental.Domain.Entities;
using BookRental.Domain.Interfaces;
using BookRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookRentalDbContext _context;

        public UserRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
