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
        private readonly ILogger<StatisticsController> _logger;

        public StatisticsController(IRentalService rentalService, ILogger<StatisticsController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        [HttpGet("most-overdue-book")]
        public async Task<IActionResult> GetMostOverdueBook()
        {
            var book = await _rentalService.GetMostOverdueBookAsync();
            if (book == null)
                return NotFound(new { message = "No overdue books found." });

            _logger.LogInformation("Most overdue book found.");
            return Ok(book);
        }

        [HttpGet("most-popular-book")]
        public async Task<IActionResult> GetMostPopularBook()
        {
            var book = await _rentalService.GetMostPopularBookAsync();
            if (book == null)
                return NotFound(new { message = "No rentals found." });

            _logger.LogInformation("Most popular book found.");
            return Ok(book);
        }

        [HttpGet("least-popular-book")]
        public async Task<IActionResult> GetLeastPopularBook()
        {
            var book = await _rentalService.GetLeastPopularBookAsync();
            if (book == null)
                return NotFound(new { message = "No rentals found." });

            _logger.LogInformation("Least popular book found.");
            return Ok(book);
        }
    }
}
