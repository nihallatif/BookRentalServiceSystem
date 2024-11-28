using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using BookRental.Application.Models;
using Newtonsoft.Json;

namespace BookRental.Tests
{
    public class RentalTests : IntegrationTestBase
    {
        public RentalTests() : base() { }

        [Fact]
        public async Task RentBook_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/rentals/rent", new { BookId = 1 });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RentBook_NoAvailableCopies_ShouldReturnBadRequest()
        {
            var response = await Client.PostAsJsonAsync("/api/rentals/rent", new
            {
                BookId = 999 // Assuming this book has no available copies
            });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturnSuccess()
        {
            var rentResponse = await Client.PostAsJsonAsync("/api/rentals/rent", new { BookId = 1 });
            rentResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await Client.PostAsJsonAsync("/api/rentals/return", new { RentalId = 1 });
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ExtendRental_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/rentals/extend", new
            {
                RentalId = 1
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ViewRentalHistory_ShouldReturnRentals()
        {
            var response = await Client.GetAsync("/api/rentals/history");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var rentals = await response.Content.ReadFromJsonAsync<dynamic>();
            rentals.Should().NotBeNull();
        }
    }
}
