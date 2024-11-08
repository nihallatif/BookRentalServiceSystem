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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public RentalService(IRentalRepository rentalRepository, IBookRepository bookRepository, 
            IWaitingListRepository waitingListRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
            _waitingListRepository = waitingListRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task RentBookAsync(int bookId, int userId)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var book = await _bookRepository.GetBookByIdAsync(bookId);
                    if (book == null)
                        throw new ArgumentException("Book not found.");

                    if (book.AvailableCopies <= 0)
                        throw new InvalidOperationException("No copies available for rent.");

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

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task ReturnBookAsync(int rentalId)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);
                    if (rental == null || rental.ReturnDate != null)
                        throw new ArgumentException("Rental not found or already returned.");

                    // Mark the book as returned
                    rental.ReturnDate = DateTime.UtcNow;
                    await _rentalRepository.UpdateRentalAsync(rental);

                    var book = await _bookRepository.GetBookByIdAsync(rental.BookId);
                    book.AvailableCopies++;
                    await _bookRepository.UpdateBookAsync(book);

                    // Notify the next user in the waiting list if any
                    var nextInLine = await _waitingListRepository.GetNextInWaitingListAsync(book.Id);
                    if (nextInLine != null)
                    {
                        // Fetch user details to send notification
                        var user = await _userRepository.GetUserByIdAsync(nextInLine.UserId);
                        if (user != null)
                        {
                            await _emailService.SendEmailAsync(
                                to: user.Username,
                                subject: "Book Available for Rent",
                                body: $"Hello {user.Username}, the book '{book.Title}' you requested is now available."
                            );

                            await _waitingListRepository.RemoveFromWaitingListAsync(nextInLine);
                        }
                    }

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
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
