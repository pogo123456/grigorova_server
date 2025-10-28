using Grigorova_Server.Models;
using Grigorova_Server.Server;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grigorova_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            var created = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, User user)
        {
            var updated = await _service.UpdateAsync(id, user);
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
        public async Task<IEnumerable<User>> SearchByName(string name) => await _service.SearchByNameAsync(name);

        [HttpGet("byemail")]
        public async Task<ActionResult<User>> ByEmail(string email)
        {
            var user = await _service.GetByEmailAsync(email);
            if (user == null) return NotFound();
            return user;
        }

        [HttpGet("byrole")]
        public async Task<IEnumerable<User>> ByRole(string role) => await _service.GetByRoleAsync(role);

        // История заимствований по имени пользователя
        [HttpGet("{userName}/history")]
        public async Task<IEnumerable<object>> BorrowingHistory(string userName)
            => await _service.GetBorrowingHistoryAsync(userName);

        // Пользователи, которые никогда не брали книги
        [HttpGet("inactive")]
        public async Task<IEnumerable<string>> InactiveUsers()
            => await _service.GetInactiveUsersAsync();
    }
}


