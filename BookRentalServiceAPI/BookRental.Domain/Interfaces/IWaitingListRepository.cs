using BookRental.Domain.Entities;

namespace BookRental.Domain.Interfaces
{
    public interface IWaitingListRepository
    {
        Task AddToWaitingListAsync(int bookId, int userId);
        Task RemoveFromWaitingListAsync(int bookId, int userId);
        Task<IEnumerable<WaitingList>> GetWaitingListForBookAsync(int bookId);
        Task<WaitingList> GetNextInWaitingListAsync(int bookId);
    }
}
