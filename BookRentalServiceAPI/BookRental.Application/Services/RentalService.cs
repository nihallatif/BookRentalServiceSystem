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
        private readonly IWaitingListRepository _waitingListRepository;

        public RentalService(IRentalRepository rentalRepository, IBookRepository bookRepository, IWaitingListRepository waitingListRepository)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
            _waitingListRepository = waitingListRepository;
        }

        public async Task RentBookAsync(int bookId, int userId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
                throw new ArgumentException("Book not found.");

            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException("No copies available for rent. The user can be added to the waiting list.");
            }

            book.AvailableCopies--;
            await _bookRepository.UpdateBookAsync(book);

            var rental = new Rental
            {
                BookId = bookId,
                UserId = userId,
                RentalDate = DateTime.UtcNow,
                IsOverdue = false
            };
            await _rentalRepository.AddRentalAsync(rental);
        }

        public async Task ReturnBookAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);
            if (rental == null || rental.ReturnDate != null)
                throw new ArgumentException("Rental not found or already returned.");

            rental.ReturnDate = DateTime.UtcNow;
            await _rentalRepository.UpdateRentalAsync(rental);

            var book = await _bookRepository.GetBookByIdAsync(rental.BookId);
            book.AvailableCopies++;
            await _bookRepository.UpdateBookAsync(book);

            var nextInLine = await _waitingListRepository.GetNextInWaitingListAsync(book.Id);
            if (nextInLine != null)
            {
                // Notify next in waiting list (optional, handled by notification service)
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

        public async Task<Book> GetMostOverdueBookAsync()
        {
            return await _rentalRepository.GetMostOverdueBookAsync();
        }

        public async Task<Book> GetMostPopularBookAsync()
        {
            return await _rentalRepository.GetMostPopularBookAsync();
        }

        public async Task<Book> GetLeastPopularBookAsync()
        {
            return await _rentalRepository.GetLeastPopularBookAsync();
        }
    }
}
