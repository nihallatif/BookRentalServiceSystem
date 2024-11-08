using BookRental.Domain.Entities;
using BookRental.Domain.Interfaces;
using BookRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Infrastructure.Repositories
{
    public class WaitingListRepository : IWaitingListRepository
    {
        private readonly BookRentalDbContext _context;

        public WaitingListRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public async Task AddToWaitingListAsync(WaitingList entry)
        {
            await _context.WaitingLists.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<WaitingList> GetNextInWaitingListAsync(int bookId)
        {
            return await _context.WaitingLists
                .Where(w => w.BookId == bookId)
                .OrderBy(w => w.RequestedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<WaitingList>> GetWaitingListByBookIdAsync(int bookId)
        {
            return await _context.WaitingLists
                .Where(w => w.BookId == bookId)
                .ToListAsync();
        }

        public async Task RemoveFromWaitingListAsync(WaitingList entry)
        {
            _context.WaitingLists.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<WaitingList> GetUserOnWaitingListAsync(int bookId, int userId)
        {
            return await _context.WaitingLists
                .FirstOrDefaultAsync(w => w.BookId == bookId && w.UserId == userId);
        }
    }
}

