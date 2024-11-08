using BookRental.Application.Common;
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
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public RentalService(IRentalRepository rentalRepository, IBookRepository bookRepository, IWaitingListRepository waitingListRepository, IUserRepository userRepository,
            IEmailService emailService)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
            _waitingListRepository = waitingListRepository;
            _userRepository = userRepository;   
            _emailService = emailService;
        }

        public async Task RentBookAsync(int bookId, int userId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
                throw new ArgumentException(Messages.BookNotFound);

            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException(Messages.AddToWaitingList);
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
                throw new ArgumentException(Messages.NoRentalsFoundOrReturned);

            // Mark the book as returned
            rental.ReturnDate = DateTime.UtcNow;
            await _rentalRepository.UpdateRentalAsync(rental);

            // Increase the available copies of the book
            var book = await _bookRepository.GetBookByIdAsync(rental.BookId);
            book.AvailableCopies++;
            await _bookRepository.UpdateBookAsync(book);

            // Check if there is a user on the waiting list for this book
            var nextInLine = await _waitingListRepository.GetNextInWaitingListAsync(book.Id);
            if (nextInLine != null)
            {
                // Fetch the user's email from the UserRepository
                var user = await _userRepository.GetUserByIdAsync(nextInLine.UserId);
                if (user != null)
                {
                    // Send an email notification to the next user in line
                    await _emailService.SendEmailAsync(
                        to: user.Username, // Assuming Username is used as email
                        subject: Messages.BookAvailableForRent,
                        body: $"Hello {user.Username},<br><br>The book '{book.Title}' you requested is now available for rent!"
                    );

                    // Remove the user from the waiting list after notifying
                    await _waitingListRepository.RemoveFromWaitingListAsync(nextInLine);
                }
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

        public async Task ExtendRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);

            if (rental == null)
                throw new ArgumentException(Messages.NoRentalsFound);

            if (rental.ExtensionCount >= 2)
                throw new InvalidOperationException(Messages.MaxRentalsReached);

            rental.ExtensionCount++;
            rental.RentalDate = rental.RentalDate.AddDays(7); // Extend by 7 days
            await _rentalRepository.UpdateRentalAsync(rental);
        }
    }
}
