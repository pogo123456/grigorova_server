using Grigorova_Server.Data;
using Grigorova_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grigorova_Server.Server
{
    public class BookService
    {
        private readonly Grigorova_ServerContext _context;

        public BookService(Grigorova_ServerContext context)
        {
            _context = context;
        }

        // ======= CRUD =======

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateAsync(int id, Book updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            book.BookTitle = updatedBook.BookTitle;
            book.GenreId = updatedBook.GenreId;
            book.ReleaseDate = updatedBook.ReleaseDate;
            book.BookPrice = updatedBook.BookPrice;
            book.BookRating = updatedBook.BookRating;

            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======= Реальные запросы =======

        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
        {
            return await _context.Books
                .Where(b => b.BookTitle.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByGenreNameAsync(string genreName)
        {
            var query = from b in _context.Books
                        join g in _context.Genres on b.GenreId equals g.GenresId
                        where g.GenresName.Contains(genreName)
                        select new
                        {
                            b.BookId,
                            b.BookTitle,
                            GenreName = g.GenresName,
                            b.ReleaseDate,
                            b.BookRating,
                            b.BookPrice
                        };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetTopRatedAsync(int count = 10)
        {
            return await _context.Books
                .OrderByDescending(b => b.BookRating)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksCheaperThanAsync(decimal price)
        {
            return await _context.Books
                .Where(b => b.BookPrice <= price)
                .ToListAsync();
        }
    }
}
