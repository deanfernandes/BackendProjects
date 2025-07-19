using ECommerceApi.Models;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Services;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UsersController(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.PasswordHash = _passwordService.HashPassword(user, user.PasswordHash);
            await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.PasswordHash = user.PasswordHash;

            await _userRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteAsync(user);

            return NoContent();
        }
    }
}