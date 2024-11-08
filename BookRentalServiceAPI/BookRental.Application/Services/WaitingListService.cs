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
    public class WaitingListService : IWaitingListService
    {
        private readonly IWaitingListRepository _waitingListRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public WaitingListService(IWaitingListRepository waitingListRepository, IUserRepository userRepository,
            IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _waitingListRepository = waitingListRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task AddToWaitingListAsync(int bookId, int userId)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Check if the user is already on the waiting list
                    var existingEntry = await _waitingListRepository.GetUserOnWaitingListAsync(bookId, userId);
                    if (existingEntry != null)
                    {
                        throw new InvalidOperationException("User is already on the waiting list for this book.");
                    }

                    var entry = new WaitingList
                    {
                        BookId = bookId,
                        UserId = userId,
                        RequestedDate = DateTime.UtcNow
                    };

                    await _waitingListRepository.AddToWaitingListAsync(entry);
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task NotifyNextUserAsync(int bookId)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    // Fetch the next user in line for this book
                    var nextInLine = await _waitingListRepository.GetNextInWaitingListAsync(bookId);
                    if (nextInLine != null)
                    {
                        // Fetch the user's email to send a notification
                        var user = await _userRepository.GetUserByIdAsync(nextInLine.UserId);
                        if (user != null)
                        {
                            await _emailService.SendEmailAsync(
                                to: user.Username,
                                subject: Messages.BookAvailableForRent,
                                body: $"Hello {user.Username}, the book you requested is now available for rent."
                            );

                            // Remove the user from the waiting list after notifying
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
    }
}
