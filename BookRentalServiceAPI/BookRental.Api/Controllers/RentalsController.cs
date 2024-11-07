using BookRental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(IRentalService rentalService, ILogger<RentalsController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] int bookId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                _logger.LogInformation("User Id not found");
                return Unauthorized(new { message = "User ID not found in token." });
            }
            await _rentalService.RentBookAsync(bookId, userId);
            _logger.LogInformation("Book rented: {BookId}", bookId);
            return Ok(new { message = "Book rented successfully." });
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] int rentalId)
        {
            await _rentalService.ReturnBookAsync(rentalId);
            _logger.LogInformation("Book returned: {Rental}", rentalId);
            return Ok(new { message = "Book returned successfully." });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRentals(int userId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int UserId))
            {
                _logger.LogInformation("User not found.");
                return Unauthorized(new { message = "User ID not found in token." });
            }
            _logger.LogInformation("Rental list of user: {UserId}", userId);
            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            return Ok(rentals);
        }
    }
}
