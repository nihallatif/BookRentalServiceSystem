using BookRental.Domain.Entities;

namespace BookRental.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);
        Task<Rental> GetRentalByIdAsync(int id);
        Task AddRentalAsync(Rental rental);
        Task UpdateRentalAsync(Rental rental);
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();

        //Statistics
        Task<Book> GetMostOverdueBookAsync();
        Task<Book> GetMostPopularBookAsync();
        Task<Book> GetLeastPopularBookAsync();
    }
}
