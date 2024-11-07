using BookRental.Application.Interfaces;
using BookRental.Domain.Entities;
using BookRental.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookRepository _bookRepository;

        public RentalService(IRentalRepository rentalRepository, IBookRepository bookRepository)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
        }

        public async Task RentBookAsync(int bookId, int userId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book.AvailableCopies > 0)
            {
                book.AvailableCopies--;
                await _bookRepository.UpdateBookAsync(book);

                var rental = new Rental { BookId = bookId, UserId = userId, RentalDate = DateTime.UtcNow };
                await _rentalRepository.AddRentalAsync(rental);
            }
            else
            {
                throw new InvalidOperationException("No copies available for rent.");
            }
        }

        public async Task ReturnBookAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);
            if (rental != null && rental.ReturnDate == null)
            {
                rental.ReturnDate = DateTime.UtcNow;
                await _rentalRepository.UpdateRentalAsync(rental);

                var book = await _bookRepository.GetBookByIdAsync(rental.BookId);
                book.AvailableCopies++;
                await _bookRepository.UpdateBookAsync(book);
            }
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await _rentalRepository.GetRentalsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            return await _rentalRepository.GetOverdueRentalsAsync();
        }
    }
}
