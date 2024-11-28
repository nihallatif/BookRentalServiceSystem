using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Models
{
    public class BookRequest
    {
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }
    }
}
