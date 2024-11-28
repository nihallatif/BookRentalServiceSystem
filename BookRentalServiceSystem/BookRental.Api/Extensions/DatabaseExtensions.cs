using BookRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRental.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddBookRentalDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookRentalDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
