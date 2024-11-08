using BookRental.Api.Extensions;
using BookRental.Api.Middleware;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/book_rental_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Define CORS policy
builder.Services.AddCorsPolicy();

// DbContext and Connection
builder.Services.AddBookRentalDbContext(builder.Configuration);

// Register repositories
builder.Services.RegisterServices();

// JWT Authentication configuration
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Host.UseSerilog();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Enable authentication and authorization middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
