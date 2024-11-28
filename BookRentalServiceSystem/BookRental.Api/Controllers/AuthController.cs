using BookRental.Application.Interfaces;
using BookRental.Application.Models;
using BookRental.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthenticationService authenticationService, IUserService userService, ILogger<AuthController> logger)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = request.Username,
                Role = request.Role // You might want to assign default role as "User"
            };

            await _userService.RegisterUserAsync(user, request.Password);
            _logger.LogInformation("Registered user: {Username}", request.Username);
            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authenticationService.AuthenticateAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogInformation("Unauthorized user: {Username}", request.Username);
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromServices] IEmailService emailService)
        {
            await emailService.SendEmailAsync("nihal.latif@gmail.com", "Test Email", "<h1>Hello from Book Rental API</h1>");
            return Ok("Email sent successfully!");
        }
    }
}
