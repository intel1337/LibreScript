using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
namespace MonApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    private readonly AppDbContext _context;
    
    public class UserController : ControllerBase
    {
        [HttpGet("get-user/{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok($"Hello from get user with id {id}!");
        }

        [HttpGet("get-user/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            return Ok($"Hello from get user with username {username}!");
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok("Hello from register!");
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok("Hello from login!");
        }
    }
}