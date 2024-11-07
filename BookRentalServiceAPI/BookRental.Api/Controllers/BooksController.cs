using BookRental.Application.Common;
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
            _logger.LogInformation(Messages.RequestGetAllBooks);

            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            _logger.LogInformation(Messages.RequestGetBook + ": {BookId}", id);

            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning(Messages.BookNotFound + ": {BookId}", id);
                return NotFound(new { message = Messages.BookNotFound });
            }

            _logger.LogInformation(Messages.BookFound + ": {BookId}", id);
            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(Messages.InvalidBookModel);
                return BadRequest(ModelState);
            }

            await _bookService.AddBookAsync(book);
            _logger.LogInformation(Messages.BookAdded + "{Title}: {BookId}", book.Title, book.Id);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] string genre)
        {
            _logger.LogInformation(Messages.SearchBookRequest + ": {Title}", title);
            var books = await _bookService.SearchBooksAsync(title, genre);
            return Ok(books);
        }
    }
}
