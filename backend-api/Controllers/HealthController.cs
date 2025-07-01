using Microsoft.AspNetCore.Mvc;

namespace MonApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Health()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                service = "LibreScript Backend API"
            });
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return Ok(new { 
                status = "ready", 
                timestamp = DateTime.UtcNow 
            });
        }
    }
} 