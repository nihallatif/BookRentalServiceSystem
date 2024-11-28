using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Tests
{
    public class WaitingListTests : IntegrationTestBase
    {
        public WaitingListTests() : base() { }

        [Fact]
        public async Task AddToWaitingList_ShouldReturnSuccess()
        {
            var response = await Client.PostAsJsonAsync("/api/rentals/waiting-list", new
            {
                BookId = 1
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddToWaitingList_UserAlreadyExists_ShouldReturnConflict()
        {
            // Add user to the waiting list for the first time
            await Client.PostAsJsonAsync("/api/rentals/waiting-list", new { BookId = 1 });

            // Try adding the same user again to the same book's waiting list
            var response = await Client.PostAsJsonAsync("/api/rentals/waiting-list", new { BookId = 1 });

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task NotifyNextUser_ShouldReturnSuccess()
        {
            // Seed the waiting list
            await Client.PostAsJsonAsync("/api/rentals/waiting-list", new { BookId = 1 });

            // Simulate book return and notify the next user
            var response = await Client.PostAsync("/api/waiting-list/notify/1", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task NotifyNextUser_NoUsersInWaitingList_ShouldReturnNotFound()
        {
            var response = await Client.PostAsync("/api/waiting-list/notify/999", null);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ExtendRental_ShouldReturnSuccess()
        {
            // First, rent a book
            await Client.PostAsJsonAsync("/api/rentals/rent", new { BookId = 1 });

            // Extend the rental
            var response = await Client.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 1 });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ExtendRental_ExceedingLimit_ShouldReturnBadRequest()
        {
            // Rent a book
            await Client.PostAsJsonAsync("/api/rentals/rent", new { BookId = 1 });

            // Extend rental twice (assuming max 2 extensions allowed)
            await Client.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 1 });
            await Client.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 1 });

            // Try to extend a third time
            var response = await Client.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 1 });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ExtendRental_NonExistentRental_ShouldReturnNotFound()
        {
            var response = await Client.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 999 });
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ExtendRental_Unauthorized_ShouldReturnUnauthorized()
        {
            // Create a new HttpClient without authentication headers
            var unauthorizedClient = new WebApplicationFactory<Program>()
                .CreateClient();

            var response = await unauthorizedClient.PostAsJsonAsync("/api/rentals/extend", new { RentalId = 1 });
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
