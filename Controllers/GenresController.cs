using Grigorova_Server.Models;
using Grigorova_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grigorova_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _service;

        public GenresController(GenreService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Genres>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Genres>> GetById(int id)
        {
            var genre = await _service.GetByIdAsync(id);
            if (genre == null) return NotFound();
            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genres>> Create(Genres genre)
        {
            var created = await _service.CreateAsync(genre);
            return CreatedAtAction(nameof(GetById), new { id = created.GenresId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Genres>> Update(int id, Genres genre)
        {
            var updated = await _service.UpdateAsync(id, genre);
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
        public async Task<IEnumerable<Genres>> SearchByName(string name) => await _service.SearchByNameAsync(name);

        [HttpGet("count")]
        public async Task<int> CountBooks(string genreName) => await _service.CountBooksInGenreAsync(genreName);
    }
}
