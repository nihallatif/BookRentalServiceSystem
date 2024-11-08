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
        public RentalTests(WebApplicationFactory<Program> factory) : base(factory) { }

        private async Task<string> GetAuthToken()
        {
            var loginRequest = new
            {
                Username = "nihal.latif@test.com",  // Replace with a valid test user
                Password = "12345678"   // Replace with the corresponding password
            };

            var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AuthenticationResponse>(loginResponseContent).Token;

            return token;
        }

        [Fact]
        public async Task RentBook_ShouldReturnSuccess()
        {
            var token = await GetAuthToken();

            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await Client.PostAsJsonAsync("/api/rentals/rent", new
            {
                BookId = 1
            });

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
            var response = await Client.PostAsJsonAsync("/api/rentals/return", new
            {
                RentalId = 1
            });

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
