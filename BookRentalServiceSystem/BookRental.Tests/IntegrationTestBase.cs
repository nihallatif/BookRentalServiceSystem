using BookRental.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace BookRental.Tests
{
    public class IntegrationTestBase : IAsyncLifetime
    {
        protected HttpClient Client;
        protected BookRentalDbContext Context;
        private readonly WebApplicationFactory<Program> _factory;
        private IServiceScope _scope;

        public IntegrationTestBase()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookRentalDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add a new in-memory database for testing
                    services.AddDbContext<BookRentalDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb-" + System.Guid.NewGuid()));
                });
            });
        }

        public async Task InitializeAsync()
        {
            _scope = _factory.Services.CreateScope();
            Context = _scope.ServiceProvider.GetRequiredService<BookRentalDbContext>();
            Client = await CreateAuthenticatedClient();
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();
            Client.Dispose();
            return Task.CompletedTask;
        }

        private async Task<HttpClient> CreateAuthenticatedClient()
        {
            var client = _factory.CreateClient();

            // Register a test user
            var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
            {
                Username = "testuser",
                Password = "Test@123",
                Role = "User"
            });

            // Ensure the registration was successful
            if (!registerResponse.IsSuccessStatusCode)
                throw new System.Exception("User registration failed.");

            // Login to get a JWT token
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
            {
                Username = "testuser",
                Password = "Test@123"
            });

            if (!loginResponse.IsSuccessStatusCode)
                throw new System.Exception("User login failed.");

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<dynamic>();
            var jwtToken = loginResult?.token;

            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
            }

            return client;
        }
    }
}
