using Grigorova_Server.Data;
using Grigorova_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grigorova_Server.Server
{
    public class AuthorService
    {
        private readonly Grigorova_ServerContext _context;

        public AuthorService(Grigorova_ServerContext context)
        {
            _context = context;
        }

        // ======= CRUD =======

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.BookAuthors)
                    .ThenInclude(ba => ba.Book)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<Author> CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAsync(int id, Author updated)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            author.AuthorFirstname = updated.AuthorFirstname;
            author.AuthorSecondname = updated.AuthorSecondname;
            author.AuthorBirthday = updated.AuthorBirthday;
            author.AuthorCountry = updated.AuthorCountry;
            author.AuthorRating = updated.AuthorRating;

            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======= Реальные запросы =======

        public async Task<IEnumerable<Author>> SearchByNameAsync(string name)
        {
            return await _context.Authors
                .Where(a =>
                    a.AuthorFirstname.ToLower().Contains(name.ToLower()) ||
                    a.AuthorSecondname.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetTopAuthorsAsync(int count = 5)
        {
            return await _context.Authors
                .OrderByDescending(a => a.AuthorRating)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetBooksByAuthorNameAsync(string? firstName, string? lastName)
        {
            var query = from ba in _context.BookAuthors
                        join b in _context.Books on ba.BookId equals b.BookId
                        join g in _context.Genres on b.GenreId equals g.GenresId
                        join a in _context.Authors on ba.AuthorId equals a.AuthorId

                        where (string.IsNullOrEmpty(firstName) || a.AuthorFirstname.Contains(firstName))
                        && (string.IsNullOrEmpty(lastName) || a.AuthorSecondname.Contains(lastName))
                        select new
                        {
                            b.BookId,
                            b.BookTitle,
                            GenreName = g.GenresName,
                            b.ReleaseDate,
                            b.BookRating,
                            b.BookPrice
                        };
            return await query.Distinct().ToListAsync();

        }

        public async Task<IEnumerable<Author>> GetAuthorsByCountryAsync(string country)
        {
            return await _context.Authors
                .Where(a => a.AuthorCountry.ToLower() == country.ToLower())
                .ToListAsync();
        }
    }
}
