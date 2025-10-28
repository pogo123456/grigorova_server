using Grigorova_Server.Models;
using Grigorova_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grigorova_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _service;

        public AuthorsController(AuthorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Author>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var author = await _service.GetByIdAsync(id);
            if (author == null) return NotFound();
            return author;
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Create(Author author)
        {
            var created = await _service.CreateAsync(author);
            return CreatedAtAction(nameof(GetById), new { id = created.AuthorId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Author>> Update(int id, Author author)
        {
            var updated = await _service.UpdateAsync(id, author);
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
        public async Task<IEnumerable<Author>> SearchByName(string name) => await _service.SearchByNameAsync(name);

        [HttpGet("top")]
        public async Task<IEnumerable<Author>> TopAuthors(int count = 5) => await _service.GetTopAuthorsAsync(count);

        [HttpGet("books")]
        public async Task<IEnumerable<object>> BooksByAuthor([FromQuery] string? firstName, [FromQuery] string? lastName) => await _service.GetBooksByAuthorNameAsync(firstName, lastName);

        [HttpGet("country")]
        public async Task<IEnumerable<Author>> ByCountry(string country) => await _service.GetAuthorsByCountryAsync(country);
    }
}
