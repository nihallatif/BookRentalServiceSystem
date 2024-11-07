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

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }
        [HttpPost("rent")]
        public async Task<IActionResult> RentBook([FromBody] int bookId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            await _rentalService.RentBookAsync(bookId, userId);
            return Ok(new { message = "Book rented successfully." });
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] int rentalId)
        {
            await _rentalService.ReturnBookAsync(rentalId);
            return Ok(new { message = "Book returned successfully." });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRentals(int userId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int UserId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            var rentals = await _rentalService.GetRentalsByUserIdAsync(userId);
            return Ok(rentals);
        }
    }
}
