using BookRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Interfaces
{
    public interface IRentalService
    {
        Task RentBookAsync(int bookId, int userId);
        Task ReturnBookAsync(int rentalId);
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();

        Task<Book> GetMostOverdueBookAsync();
        Task<Book> GetMostPopularBookAsync();
        Task<Book> GetLeastPopularBookAsync();
    }
}
