﻿using BookRental.Application.Interfaces;
using BookRental.Domain.Entities;
using BookRental.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRental.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book?>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddBookAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateBookAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string title, string genre)
        {
            return await _bookRepository.SearchBooksAsync(title, genre);
        }
    }
}
