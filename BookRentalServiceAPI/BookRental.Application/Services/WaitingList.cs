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
        private readonly IBookRepository _bookRepository;

        public WaitingListService(IWaitingListRepository waitingListRepository, IBookRepository bookRepository)
        {
            _waitingListRepository = waitingListRepository;
            _bookRepository = bookRepository;
        }

        public async Task AddToWaitingListAsync(int bookId, int userId)
        {
            await _waitingListRepository.AddToWaitingListAsync(bookId, userId);
        }

        public async Task RemoveFromWaitingListAsync(int bookId, int userId)
        {
            await _waitingListRepository.RemoveFromWaitingListAsync(bookId, userId);
        }

        public async Task<IEnumerable<WaitingList>> GetWaitingListForBookAsync(int bookId)
        {
            return await _waitingListRepository.GetWaitingListForBookAsync(bookId);
        }

        public async Task NotifyNextInWaitingListAsync(int bookId)
        {
            var nextUser = await _waitingListRepository.GetNextInWaitingListAsync(bookId);
            if (nextUser != null)
            {
                // Notify user (notification service can be injected and called here)
                await _waitingListRepository.RemoveFromWaitingListAsync(bookId, nextUser.UserId);
            }
        }
    }
}
