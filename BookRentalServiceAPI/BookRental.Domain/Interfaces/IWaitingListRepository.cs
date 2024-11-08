using BookRental.Domain.Entities;

namespace BookRental.Domain.Interfaces
{
    public interface IWaitingListRepository
    {
        Task AddToWaitingListAsync(WaitingList entry);
        Task<WaitingList> GetNextInWaitingListAsync(int bookId);
        Task<IEnumerable<WaitingList>> GetWaitingListByBookIdAsync(int bookId);
        Task RemoveFromWaitingListAsync(WaitingList entry);
    }
}
