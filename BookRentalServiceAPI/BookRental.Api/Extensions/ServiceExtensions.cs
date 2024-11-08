using BookRental.Api.BackgroundServices;
using BookRental.Api.Middleware;
using BookRental.Application.Interfaces;
using BookRental.Application.Services;
using BookRental.Domain.Interfaces;
using BookRental.Infrastructure.Repositories;

namespace BookRental.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IWaitingListRepository, WaitingListRepository>();

            // Register services
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IWaitingListService, WaitingListService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddHostedService<OverdueRentalNotificationService>();
            services.AddTransient<ExceptionHandlingMiddleware>();

            return services;
        }
    }
}
