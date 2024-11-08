using BookRental.Application.Common;
using BookRental.Application.Interfaces;
using BookRental.Application.Models;
using BookRental.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IWaitingListService _waitingListService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IRentalService rentalService, IWaitingListService waitingListService, ILogger<RentalsController> logger)
        {
            _rentalService = rentalService;
            _waitingListService = waitingListService;
            _logger = logger;
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] RentBookRequest request)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                _logger.LogInformation(Messages.UserNotFound);
                return Unauthorized(new { message = Messages.UserNotFound });
            }
            await _rentalService.RentBookAsync(request.BookId, userId);
            _logger.LogInformation(Messages.BookRentedSuccessfully + ": {BookId}", request.BookId);
            return Ok(new { message = Messages.BookRentedSuccessfully });
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] int rentalId)
        {
            await _rentalService.ReturnBookAsync(rentalId);
            _logger.LogInformation(Messages.BookReturnedSuccessfully + ": {Rental}", rentalId);
            return Ok(new { message = Messages.BookReturnedSuccessfully });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRentals(int userId)
        {
            _logger.LogInformation(Messages.UserRentalList + ": {UserId}", userId);
            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            return Ok(rentals);
        }

        [HttpPost("waiting-list")]
        public async Task<IActionResult> AddToWaitingList([FromBody] int bookId)
        {
            int userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            await _waitingListService.AddToWaitingListAsync(bookId, userId);
            return Ok(new { message = "Added to waiting list successfully." });
        }

        [HttpPost("extend")]
        public async Task<IActionResult> ExtendRental([FromBody] int rentalId)
        {
            await _rentalService.ExtendRentalAsync(rentalId);
            return Ok(new { message = "Rental extended successfully." });
        }
    }
}
