using BookRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Infrastructure.Data
{
    public class BookRentalDbContext : DbContext
    {
        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure any additional settings here if needed
            base.OnModelCreating(modelBuilder);

            // Seed dummy users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "adminpasswordhash", Role = "Admin" },
                new User { Id = 2, Username = "user1", PasswordHash = "userpasswordhash", Role = "User" }
            );

            // Seed sample books
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Sample Book 1", Author = "Author 1", ISBN = "1234567890123", Genre = "Fiction", AvailableCopies = 5 },
                new Book { Id = 2, Title = "Sample Book 2", Author = "Author 2", ISBN = "9876543210987", Genre = "Non-Fiction", AvailableCopies = 3 }
            );
        }
    }
}
