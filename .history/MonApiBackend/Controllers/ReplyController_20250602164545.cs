using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Entities;
using MonApiBackend.Models.Context;


namespace MonApiBackend.Controllers
{
    public class ReplyController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        
        

    }
}
