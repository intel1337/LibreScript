using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;

namespace MonApiBackend.Controllers
{
    public class Category : Controller
    {
        private readonly AppDbContext _context;
        public Category(AppDbContext context)
        {
            _context = context; // Instance le contexte de la base de données, _ pour préciser un attribut privé de la class

        }
        [HttpGet("get-category/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var result = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(result);
        }

    }
}
