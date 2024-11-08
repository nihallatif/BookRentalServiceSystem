using BookRental.Domain.Entities;

namespace BookRental.Application.Interfaces
{
    public interface IRentalService
    {
        Task RentBookAsync(int bookId, int userId);
        Task ReturnBookAsync(int rentalId);
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();
        Task ExtendRentalAsync(int rentalId);

        Task<Book> GetMostOverdueBookAsync();
        Task<Book> GetMostPopularBookAsync();
        Task<Book> GetLeastPopularBookAsync();
    }
}
