using BookRental.Application.Interfaces;
using BookRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Require authentication for all actions in this controller
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation("Request received to get all books");

            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            _logger.LogInformation("Request received to get book with ID: {BookId}", id);

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found.", id);
                return NotFound(new { message = "Book not found." });
            }

            _logger.LogInformation("Successfully retrieved book with ID: {BookId}", id);
            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for book creation.");
                return BadRequest(ModelState);
            }

            await _bookService.AddBookAsync(book);
            _logger.LogInformation("Book '{Title}' created successfully with ID: {BookId}", book.Title, book.Id);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] string genre)
        {
            _logger.LogInformation("Request received to search book with: {Title}", title);
            var books = await _bookService.SearchBooksAsync(title, genre);
            return Ok(books);
        }
    }
}
