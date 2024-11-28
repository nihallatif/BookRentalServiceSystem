using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "RentalDate is required")]
        public DateTime RentalDate { get; set; }
        
        public DateTime? ReturnDate { get; set; }
        public bool IsOverdue { get; set; }
        public int ExtensionCount { get; set; } = 0;  // Limits rental extensions
        public byte[]? RowVersion { get; set; } // Add this line to handle concurrency

        // Navigation properties
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
