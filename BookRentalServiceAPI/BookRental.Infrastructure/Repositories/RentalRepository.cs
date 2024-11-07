using BookRental.Domain.Entities;
using BookRental.Domain.Interfaces;
using BookRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly BookRentalDbContext _context;

        public RentalRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> GetRentalByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await _context.Rentals
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .ToListAsync();
        }

        public async Task AddRentalAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            return await _context.Rentals
                .Where(r => r.IsOverdue && r.ReturnDate == null)
                .Include(r => r.Book)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}
