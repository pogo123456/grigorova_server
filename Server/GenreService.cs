using Grigorova_Server.Data;
using Grigorova_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grigorova_Server.Server
{
    public class GenreService
    {
        private readonly Grigorova_ServerContext _context;

        public GenreService(Grigorova_ServerContext context)
        {
            _context = context;
        }

        // ======= CRUD =======

        public async Task<IEnumerable<Genres>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genres?> GetByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<Genres> CreateAsync(Genres genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genres?> UpdateAsync(int id, Genres updated)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return null;

            genre.GenresName = updated.GenresName;
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======= Реальные запросы =======

        public async Task<IEnumerable<Genres>> SearchByNameAsync(string name)
        {
            return await _context.Genres
                .Where(g => g.GenresName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<int> CountBooksInGenreAsync(string name)
        {
            return await (from b in _context.Books
                          join g in _context.Genres on b.GenreId equals g.GenresId
                          where g.GenresName == name
                          select b).CountAsync();
        }
    }
}
