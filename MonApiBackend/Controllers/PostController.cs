using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;

namespace MonApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        // Get all posts
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _context.Posts.ToList();
            return Ok(posts);
        }

        // Get a single post by ID
        [HttpGet("{id:int}")]
        public IActionResult GetPostById(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound($"Post with ID {id} not found.");
            return Ok(post);
        }

        // Create a new post
        [HttpPost]
        public IActionResult CreatePost([FromBody] Post post)
        {
            if (post == null)
                return BadRequest("Post data is required.");

            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found.");
            post.User = user;

            _context.Posts.Add(post);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        // Update an existing post
        [HttpPut("{id:int}")]
        public IActionResult UpdatePost(int id, [FromBody] Post post)
        {
            if (post == null || id != post.Id)
                return BadRequest("Post data is invalid.");

            var existingPost = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (existingPost == null)
                return NotFound($"Post with ID {id} not found.");

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;

            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId);
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found.");
            existingPost.User = user;

            _context.SaveChanges();
            return NoContent();
        }

        // Delete a post
        [HttpDelete("{id:int}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound($"Post with ID {id} not found.");

            _context.Posts.Remove(post);
            _context.SaveChanges();
            return NoContent();
        }
    }
}