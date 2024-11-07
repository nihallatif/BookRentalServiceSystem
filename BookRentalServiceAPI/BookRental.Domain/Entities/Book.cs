using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(100, ErrorMessage = "Author cannot exceed 100 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [RegularExpression(@"\d{13}", ErrorMessage = "ISBN must be 13 digits.")]
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int AvailableCopies { get; set; }  // Tracks how many copies are available for rent
    }
}
