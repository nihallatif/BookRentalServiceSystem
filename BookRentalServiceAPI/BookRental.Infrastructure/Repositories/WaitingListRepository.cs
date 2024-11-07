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

        public async Task AddToWaitingListAsync(int bookId, int userId)
        {
            var entry = new WaitingList { BookId = bookId, UserId = userId, RequestedDate = DateTime.UtcNow };
            await _context.WaitingLists.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromWaitingListAsync(int bookId, int userId)
        {
            var entry = await _context.WaitingLists.FirstOrDefaultAsync(w => w.BookId == bookId && w.UserId == userId);
            if (entry != null)
            {
                _context.WaitingLists.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WaitingList>> GetWaitingListForBookAsync(int bookId)
        {
            return await _context.WaitingLists
                .Where(w => w.BookId == bookId)
                .OrderBy(w => w.RequestedDate)
                .ToListAsync();
        }

        public async Task<WaitingList> GetNextInWaitingListAsync(int bookId)
        {
            return await _context.WaitingLists
                .Where(w => w.BookId == bookId)
                .OrderBy(w => w.RequestedDate)
                .FirstOrDefaultAsync();
        }
    }
}
