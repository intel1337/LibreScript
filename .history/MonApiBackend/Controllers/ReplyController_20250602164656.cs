using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;

namespace MonApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReplyController(AppDbContext context)
        {
            _context = context;
        }
        
        // You can now add your API methods here
    }
}