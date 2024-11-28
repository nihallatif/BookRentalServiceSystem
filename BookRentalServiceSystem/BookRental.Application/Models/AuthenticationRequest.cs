using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Models
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        //Constructor-Based Initialization
        public AuthenticationRequest(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username), "Username cannot be null.");
            Password = password ?? throw new ArgumentNullException(nameof(password), "Password cannot be null.");
        }
    }
}
