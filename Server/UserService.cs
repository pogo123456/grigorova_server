using Grigorova_Server.Data;
using Grigorova_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grigorova_Server.Server
{
    public class UserService
    {
        private readonly Grigorova_ServerContext _context;

        public UserService(Grigorova_ServerContext context)
        {
            _context = context;
        }

        // ======= CRUD =======

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return null;

            existing.UserName = user.UserName;
            existing.UserMail = user.UserMail;
            existing.UserBirthday = user.UserBirthday;
            existing.UserRole = user.UserRole;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // ======= Реальные запросы =======

        public async Task<IEnumerable<User>> SearchByNameAsync(string name)
        {
            return await _context.Users
                .Where(u => u.UserName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserMail.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.UserRole.ToLower() == role.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetBorrowingHistoryAsync(string userName)
        {
            return await (from b in _context.Borrowings
                          join u in _context.Users on b.UserId equals u.UserId
                          join bk in _context.Books on b.BookId equals bk.BookId
                          where u.UserName == userName
                          orderby b.StartReading descending
                          select new
                          {
                              b.BorrowId,
                              BookTitle = bk.BookTitle,
                              b.StartReading,
                              b.EndReading,
                              b.BorrowStatus
                          }).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetInactiveUsersAsync()
        {
            var activeUserIds = await _context.Borrowings
                .Select(b => b.UserId)
                .Distinct()
                .ToListAsync();

            return await _context.Users
                .Where(u => !activeUserIds.Contains(u.UserId))
                .Select(u => u.UserName)
                .ToListAsync();
        }
    }
}
