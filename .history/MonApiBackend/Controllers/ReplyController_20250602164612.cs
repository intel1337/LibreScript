using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;

namespace MonApiBackend.Controllers
{
    public class ReplyController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        private readonly AppDbContext _context;

        public ReplyController(AppDbContext context)
        {
            _context = context;
        }
        
        

    }
}
