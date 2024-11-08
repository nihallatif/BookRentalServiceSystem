using BookRental.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Tests
{
    public class BookTests : IntegrationTestBase
    {
        public BookTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task AddBook_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/books", new
            {
                Title = "New Book",
                Author = "Author Name",
                AvailableCopies = 5
            });

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddBook_WithMissingFields_ShouldReturnBadRequest()
        {
            var response = await Client.PostAsJsonAsync("/api/books", new
            {
                Title = "Incomplete Book"
                // Missing Author and AvailableCopies
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddBook_WithDuplicateTitle_ShouldReturnConflict()
        {
            // Add the book for the first time
            await Client.PostAsJsonAsync("/api/books", new
            {
                Title = "Duplicate Book",
                Author = "Author",
                AvailableCopies = 3
            });

            // Try to add the same book again
            var response = await Client.PostAsJsonAsync("/api/books", new
            {
                Title = "Duplicate Book",
                Author = "Author",
                AvailableCopies = 3
            });

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnBookList()
        {
            // Seed test data
            await Context.Books.AddRangeAsync(new List<Book>
            {
                new Book { Title = "Book1", Author = "Author1", AvailableCopies = 2 },
                new Book { Title = "Book2", Author = "Author2", AvailableCopies = 1 }
            });
            await Context.SaveChangesAsync();

            var response = await Client.GetAsync("/api/books");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            books.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task SearchBooks_ShouldReturnCorrectResults()
        {
            var response = await Client.GetAsync("/api/books/search?title=1984&genre=Dystopian");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            books.Should().NotBeEmpty();
        }

        [Fact]
        public async Task BookById_ShouldReturnCorrectResults()
        {
            var response = await Client.GetAsync("/api/books/1");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            books.Should().NotBeEmpty();
        }

    }
}
