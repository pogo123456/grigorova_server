using Grigorova_Server.Models;
using Grigorova_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grigorova_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;

        public BooksController(BookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            var created = await _service.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = created.BookId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update(int id, Book book)
        {
            var updated = await _service.UpdateAsync(id, book);
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
        [HttpGet("search")]
        public async Task<IEnumerable<Book>> SearchByTitle(string title) => await _service.SearchByTitleAsync(title);

        [HttpGet("genre")]
        public async Task<IEnumerable<object>> ByGenre(string genreName) => await _service.GetByGenreNameAsync(genreName);

        [HttpGet("top")]
        public async Task<IEnumerable<Book>> TopRated(int count = 10) => await _service.GetTopRatedAsync(count);

        [HttpGet("cheaperthan/{price}")]
        public async Task<IEnumerable<Book>> CheaperThan(decimal price) => await _service.GetBooksCheaperThanAsync(price);
    }
}
