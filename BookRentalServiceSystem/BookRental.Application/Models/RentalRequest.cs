using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Models
{
    public class RentalRequest
    {
        [Required(ErrorMessage = "RentalId is required")]
        public int RentalId { get; set; }
    }
}
