using BookRental.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookRental.Tests
{
    public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;
        protected readonly BookRentalDbContext Context;

        public IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            var webAppFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookRentalDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add an in-memory database for testing
                    services.AddDbContext<BookRentalDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb"));
                });
            });

            Client = webAppFactory.CreateClient();
            Context = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<BookRentalDbContext>();
        }
    }
}
