using BookRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Interfaces
{
    public interface IWaitingListService
    {
        Task AddToWaitingListAsync(int bookId, int userId);
        Task NotifyNextUserAsync(int bookId);
    }
}
