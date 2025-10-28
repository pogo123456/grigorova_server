using Grigorova_Server.Data;
using Grigorova_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grigorova_Server.Server
{
    public class BorrowingService
    {
        private readonly Grigorova_ServerContext _context;

        public BorrowingService(Grigorova_ServerContext context)
        {
            _context = context;
        }

        // ======= CRUD =======

        public async Task<IEnumerable<Borrowings>> GetAllAsync()
        {
            return await _context.Borrowings.ToListAsync();
        }

        public async Task<Borrowings?> GetByIdAsync(int id)
        {
            return await _context.Borrowings.FirstOrDefaultAsync(b => b.BorrowId == id);
        }

        public async Task<Borrowings> CreateAsync(Borrowings borrowing)
        {
            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<Borrowings?> UpdateAsync(int id, Borrowings updated)
        {
            var existing = await _context.Borrowings.FindAsync(id);
            if (existing == null) return null;

            existing.UserId = updated.UserId;
            existing.BookId = updated.BookId;
            existing.StartReading = updated.StartReading;
            existing.EndReading = updated.EndReading;
            existing.BorrowStatus = updated.BorrowStatus;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing == null) return false;

            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======= Реалистичные методы =======

        public async Task<Borrowings?> StartReadingAsync(int userId, int bookId)
        {
            bool alreadyReading = await _context.Borrowings
                .AnyAsync(b => b.UserId == userId && b.BookId == bookId && b.BorrowStatus == "Читается");

            if (alreadyReading)
                throw new InvalidOperationException("Пользователь уже читает эту книгу.");

            var borrowing = new Borrowings
            {
                UserId = userId,
                BookId = bookId,
                StartReading = DateTime.Now,
                BorrowStatus = "Читается"
            };

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<Borrowings?> FinishReadingAsync(int userId, int bookId)
        {
            var borrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId && b.BorrowStatus == "Читается");

            if (borrowing == null)
                throw new InvalidOperationException("Эта книга не числится как читаемая.");

            borrowing.BorrowStatus = "Закончена";
            borrowing.EndReading = DateTime.Now;

            await _context.SaveChangesAsync();
            return borrowing;
        }

        public async Task<IEnumerable<Borrowings>> GetActiveReadingsAsync(int userId)
        {
            return await _context.Borrowings
                .Where(b => b.UserId == userId && b.BorrowStatus == "Читается")
                .ToListAsync();
        }


        public async Task<IEnumerable<Borrowings>> GetAllCurrentlyReadingAsync()
        {
            return await _context.Borrowings
                .Where(b => b.BorrowStatus == "Читается")
                .OrderBy(b => b.StartReading)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetMostReadBooksAsync(int count = 5)
        {
            return await _context.Borrowings
                .Where(b => b.BorrowStatus == "Закончена")
                .GroupBy(b => b.BookId)
                .Select(g => new
                {
                    BookId = g.Key,
                    TimesRead = g.Count()
                })
                .OrderByDescending(x => x.TimesRead)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetMostActiveReadersAsync(int count = 5)
        {
            return await _context.Borrowings
                .Where(b => b.BorrowStatus == "Закончена")
                .GroupBy(b => b.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    BooksRead = g.Count()
                })
                .OrderByDescending(x => x.BooksRead)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> IsUserReadingAsync(int userId, int bookId)
        {
            return await _context.Borrowings
                .AnyAsync(b => b.UserId == userId && b.BookId == bookId && b.BorrowStatus == "Читается");
        }

        public async Task<object> GetReadingStatisticsAsync()
        {
            var total = await _context.Borrowings.CountAsync();
            var read = await _context.Borrowings.CountAsync(b => b.BorrowStatus == "Закончена");
            var reading = await _context.Borrowings.CountAsync(b => b.BorrowStatus == "Читается");

            return new
            {
                Total = total,
                Reading = reading,
                Read = read,
                ReadingPercent = total == 0 ? 0 : (double)reading / total * 100,
                ReadPercent = total == 0 ? 0 : (double)read / total * 100
            };
        }
    }
}
