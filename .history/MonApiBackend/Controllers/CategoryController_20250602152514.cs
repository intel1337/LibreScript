using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using MonApiBackend.m

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
        [HttpGet("get-all-categories")]
        public IActionResult GetAllCategories()
        {
            var result = _context.Categories.ToList();
            if (result.Count == 0)
            {
                return NotFound("No categories found.");
            }
            return Ok(result);
        }
        [HttpPost("create-category")]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

    }
}
