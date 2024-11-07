using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Domain.Entities
{
    public class WaitingList
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime RequestedDate { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public User User { get; set; }
    }
}
