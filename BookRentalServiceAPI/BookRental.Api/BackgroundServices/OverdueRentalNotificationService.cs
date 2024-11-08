using BookRental.Application.Common;
using BookRental.Application.Interfaces;
using BookRental.Application.Models;
using BookRental.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace BookRental.Api.BackgroundServices
{
    public class OverdueRentalNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RentalSettings _rentalSettings;
        private readonly ILogger<OverdueRentalNotificationService> _logger;

        public OverdueRentalNotificationService(IServiceProvider serviceProvider, IOptions<RentalSettings> rentalSettings, ILogger<OverdueRentalNotificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _rentalSettings = rentalSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(Messages.OverdueRentalCheck);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var rentalRepository = scope.ServiceProvider.GetRequiredService<IRentalRepository>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var overdueRentals = await rentalRepository.GetOverdueRentalsAsync();

                    foreach (var rental in overdueRentals)
                    {
                        var userEmail = rental.User.Username;  // Assuming User entity has an Email property
                        var bookTitle = rental.Book.Title;
                        var subject = Messages.OverdueRentalReminder;
                        var body = $"Dear Customer,<br><br>Your rental for '{bookTitle}' is overdue. Please return it as soon as possible to avoid further penalties.";

                        await emailService.SendEmailAsync(userEmail, subject, body);
                        _logger.LogInformation(Messages.OverdueNotificationSent + "to {UserEmail}", userEmail);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Runs daily
            }
        }

        private async Task UpdateOverdueRentalsAsync(IRentalRepository rentalRepository)
        {
            var today = DateTime.UtcNow;
            var overdueRentals = await rentalRepository.GetActiveRentalsAsync();

            foreach (var rental in overdueRentals)
            {
                // Calculate if the rental is overdue
                var dueDate = rental.RentalDate.AddDays(_rentalSettings.DefaultRentalPeriodInDays); 
                if (rental.ReturnDate == null && today > dueDate)
                {
                    rental.IsOverdue = true;
                    await rentalRepository.UpdateRentalAsync(rental);
                    _logger.LogInformation($"Marked rental {rental.Id} as overdue.");
                }
            }
        }
    }
}
