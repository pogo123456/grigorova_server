using Grigorova_Server.Models;
using Grigorova_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grigorova_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingsController : ControllerBase
    {
        private readonly BorrowingService _service;

        public BorrowingsController(BorrowingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Borrowings>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Borrowings>> GetById(int id)
        {
            var borrowing = await _service.GetByIdAsync(id);
            if (borrowing == null) return NotFound();
            return borrowing;
        }

        [HttpPost]
        public async Task<ActionResult<Borrowings>> Create(Borrowings borrowing)
        {
            var created = await _service.CreateAsync(borrowing);
            return CreatedAtAction(nameof(GetById), new { id = created.BorrowId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Borrowings>> Update(int id, Borrowings borrowing)
        {
            var updated = await _service.UpdateAsync(id, borrowing);
            if (updated == null) return NotFound();
            return updated;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // Дополнительные методы
        [HttpPost("start")]
        public async Task<ActionResult<Borrowings>> StartReading(int userId, int bookId)
        {
            try
            {
                var borrowing = await _service.StartReadingAsync(userId, bookId);
                return borrowing;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("finish")]
        public async Task<ActionResult<Borrowings>> FinishReading(int userId, int bookId)
        {
            try
            {
                var borrowing = await _service.FinishReadingAsync(userId, bookId);
                return borrowing;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("active/{userId}")]
        public async Task<IEnumerable<Borrowings>> ActiveReadings(int userId) => await _service.GetActiveReadingsAsync(userId);

        [HttpGet("currentlyreading")]
        public async Task<IEnumerable<Borrowings>> CurrentlyReading() => await _service.GetAllCurrentlyReadingAsync();

        [HttpGet("mostread")]
        public async Task<IEnumerable<object>> MostReadBooks(int count = 5) => await _service.GetMostReadBooksAsync(count);

        [HttpGet("mostactive")]
        public async Task<IEnumerable<object>> MostActiveReaders(int count = 5) => await _service.GetMostActiveReadersAsync(count);

        [HttpGet("statistics")]
        public async Task<object> ReadingStatistics() => await _service.GetReadingStatisticsAsync();
    }
}
