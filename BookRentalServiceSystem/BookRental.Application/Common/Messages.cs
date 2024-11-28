using BookRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Common
{
    public sealed class Messages
    {
        // User-related messages
        public const string UserNotFound = "User not found.";
        public const string UserRegisteredSuccessfully = "User registered successfully.";
        public const string UserUpdatedSuccessfully = "User updated successfully.";
        public const string InvalidUsernameOrPassword = "Invalid username or password";

        // Book-related messages
        public const string BookAvailableForRent = "Book Available Notification";
        public const string BookNotAvailableForRent = "Book not available for rent.";
        public const string BookRentedSuccessfully = "Book rented successfully.";
        public const string BookReturnedSuccessfully = "Book returned successfully.";
        public const string BookNotFound = "Book not found.";
        public const string UserRentalList = "Rental list of user.";
        public const string RequestGetAllBooks = "Request received to get all books";
        public const string RequestGetBook = "Request received to get book with ID";
        public const string BookFound = "Successfully retrieved book with ID";
        public const string InvalidBookModel = "Invalid model state for book creation.";
        public const string BookAdded = "New Book added successfully";
        public const string SearchBookRequest = "Request received to search book with";
        public const string NoOverdueBookFound = "Overdue Book not found.";
        public const string MostOverdueBookFound = "Most overdue book found.";
        public const string MostPopularBookFound = "Most popular book found.";
        public const string LeastPopularBookFound = "Least popular book found.";

        // Waiting list messages
        public const string AddedToWaitingList = "Added to waiting list successfully.";
        public const string RemovedFromWaitingList = "Removed from waiting list successfully.";
        public const string AddToWaitingList = "No copies available for rent. The user can be added to the waiting list.";

        // Overdue-related messages
        public const string NoOverdueRentalsFound = "No overdue rentals found.";
        public const string OverdueNotificationSent = "Overdue notification sent successfully.";
        public const string NoRentalsFound = "No rentals found.";
        public const string NoRentalsFoundOrReturned = "No rentals found or already returned.";
        public const string OverdueRentalCheck = "Running overdue rental check...";
        public const string OverdueRentalReminder = "Overdue rental reminder";

        // Rental extension messages
        public const string RentalExtendedSuccessfully = "Rental extended successfully.";
        public const string RentalExtensionNotAllowed = "Maximum extension limit reached.";
        public const string InvalidRentalId = "Invalid rental ID or book already returned.";
        public const string MaxRentalsReached = "Maximum rental extensions reached.";

        // General error messages
        public const string InvalidOperation = "Invalid operation.";
        public const string InternalServerError = "An error occurred while processing your request.";

        //Email messages
        public const string EmailSent = "Email sent successfully to";
        public const string EmailError = "Error occurred while sending email to";
    }
}
