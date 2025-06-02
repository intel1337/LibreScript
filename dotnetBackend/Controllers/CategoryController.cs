using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Entities;
using MonApiBackend.Models.Context;
using System.Linq;

namespace MonApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-category/{id:int}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            return Ok(category);
        }

        [HttpGet("get-all-categories/")]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Categories.ToList();
            if (!categories.Any())
                return NotFound("No categories found.");

            return Ok(categories);
        }

        [HttpPost("create-category/")]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("Category data is required.");

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
    }
}