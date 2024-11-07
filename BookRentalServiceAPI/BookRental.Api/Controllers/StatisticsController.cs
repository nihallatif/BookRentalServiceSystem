using BookRental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]  // Only accessible to Admins
    public class StatisticsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public StatisticsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("most-overdue-book")]
        public async Task<IActionResult> GetMostOverdueBook()
        {
            var book = await _rentalService.GetMostOverdueBookAsync();
            if (book == null)
                return NotFound(new { message = "No overdue books found." });

            return Ok(book);
        }

        [HttpGet("most-popular-book")]
        public async Task<IActionResult> GetMostPopularBook()
        {
            var book = await _rentalService.GetMostPopularBookAsync();
            if (book == null)
                return NotFound(new { message = "No rentals found." });

            return Ok(book);
        }

        [HttpGet("least-popular-book")]
        public async Task<IActionResult> GetLeastPopularBook()
        {
            var book = await _rentalService.GetLeastPopularBookAsync();
            if (book == null)
                return NotFound(new { message = "No rentals found." });

            return Ok(book);
        }
    }
}
