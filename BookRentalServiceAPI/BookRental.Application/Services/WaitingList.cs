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
        private readonly IEmailService _emailService;

        public WaitingListService(IWaitingListRepository waitingListRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _waitingListRepository = waitingListRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task AddToWaitingListAsync(int bookId, int userId)
        {
            var entry = new WaitingList
            {
                BookId = bookId,
                UserId = userId,
                RequestedDate = DateTime.UtcNow
            };

            await _waitingListRepository.AddToWaitingListAsync(entry);
        }

        public async Task NotifyNextUserAsync(int bookId)
        {
            var nextInLine = await _waitingListRepository.GetNextInWaitingListAsync(bookId);
            if (nextInLine != null)
            {
                // Fetch the user details using UserId
                var user = await _userRepository.GetUserByIdAsync(nextInLine.UserId);

                if (user != null)
                {
                    // Send email to the user's email (stored in Username field)
                    await _emailService.SendEmailAsync(
                        to: user.Username, // Using Username as Email
                        subject: Messages.BookAvailableForRent,
                        body: $"Hello {user.Username},<br><br>The book you requested is now available for rent!"
                    );

                    // Remove the user from the waiting list after notifying
                    await _waitingListRepository.RemoveFromWaitingListAsync(nextInLine);
                }
            }
        }
    }
}
