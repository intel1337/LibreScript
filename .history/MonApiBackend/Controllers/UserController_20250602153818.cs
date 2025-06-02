using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using MonApiBackend.Models;
namespace MonApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("get-all-users/")]

        [HttpGet("get-user/{id}")]
        public IActionResult GetUserById(int id)
        {
            var result = _context.Users.FirstOrDefault(u => u.Id == id);
            return Ok(result);
        }

        [HttpGet("get-user/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            return Ok($"Hello from get user with username {username}!");
        }

        [HttpPost("register/")]
        public IActionResult Register()
        {
            return Ok("Hello from register!");
        }

        [HttpPost("login/")]
        public IActionResult Login()
        {
            return Ok("Hello from login!");
        }
    }
}