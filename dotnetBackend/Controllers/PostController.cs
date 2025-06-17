using Microsoft.AspNetCore.Http; // Import pour la gestion des requêtes HTTP
using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MonApiBackend.Controllers
{
    [Route("api/[controller]")] // Définit la route de base pour ce contrôleur, ici "api/post"
    [ApiController] // Indique que cette classe est un contrôleur API
    public class PostController : ControllerBase
    {
        // Instance privée du contexte de la base de données (hérite de DbContext)
        private readonly AppDbContext _context;

        // Constructeur du contrôleur, injection du contexte de la base de données
        public PostController(AppDbContext context)
        {
            _context = context; // _ pour indiquer une variable privée
        }

        // Renvoie tous les posts Route de base en GET
        // Route: GET api/post
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            // SQL équivalent : SELECT * FROM Posts;
            var posts = _context.Posts.ToList(); // Récupère tous les posts
            // On retourne les champs principaux + nouveaux champs
            var result = posts.Select(p => new {
                p.Id,
                p.Title,
                p.Content,
                p.UserId,
                p.Upvotes,
                p.Downvotes,
                p.Language,
                p.Status,
                p.Views,
                p.CreatedAt
            });
            return Ok(result); // Retourne la liste avec un code 200 OK
        }

        // Renvoie un post par son id
        // Route: GET api/post/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetPostById(int id)
        {
            var currentUserIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = null;
            if (!string.IsNullOrEmpty(currentUserIdString))
            {
                int.TryParse(currentUserIdString, out int userId);
                currentUserId = userId;
            }

            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound($"Post with ID {id} not found.");

            // Récupérer le vote de l'utilisateur si connecté
            bool? userVote = null;
            if (currentUserId.HasValue)
            {
                var vote = _context.PostVotes.FirstOrDefault(v => v.PostId == id && v.UserId == currentUserId.Value);
                if (vote != null)
                {
                    userVote = vote.IsUpvote;
                }
            }

            // Créer un objet anonyme avec les informations du post et le vote de l'utilisateur
            var result = new
            {
                post.Id,
                post.Title,
                post.Content,
                post.UserId,
                post.Upvotes,
                post.Downvotes,
                post.Language,
                post.Status,
                post.Views,
                post.CreatedAt,
                userVote
            };

            return Ok(result);
        }

        // Crée un nouveau post Route de base en POST
        // Route: POST api/post
        [HttpPost]
        public IActionResult CreatePost([FromBody] Post post)
        {
            if (post == null)
                return BadRequest("Post data is required."); // 400 si aucune donnée reçue

            // SQL équivalent : SELECT * FROM Users WHERE Id = {post.UserId};
            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId); // Vérifie que l'utilisateur existe
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found."); // 400 si l'utilisateur n'existe pas
            post.User = user;
            post.Upvotes = 0; // Initialise à 0
            post.Downvotes = 0; // Initialise à 0
            post.Views = 0; // Initialise à 0
            post.Status = post.Status ?? "";
            post.Language = post.Language ?? "";
            post.CreatedAt = DateTime.UtcNow;

            // SQL équivalent : INSERT INTO Posts (Title, Content, UserId, Upvotes, Downvotes, Views, Status, Language, CreatedAt) VALUES (...);
            _context.Posts.Add(post); // Ajoute le post à la base
            _context.SaveChanges(); // Sauvegarde les changements
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post); // Retourne 201 Created avec l'URL du post
        }

        // Met à jour un post existant
        // Route: PUT api/post/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdatePost(int id, [FromBody] Post post)
        {
            if (post == null || id != post.Id)
                return BadRequest("Post data is invalid."); // 400 si l'id ne correspond pas ou données invalides

            // SQL équivalent : SELECT * FROM Posts WHERE Id = {id};
            var existingPost = _context.Posts.FirstOrDefault(p => p.Id == id); // Cherche le post existant
            if (existingPost == null)
                return NotFound($"Post with ID {id} not found."); // 404 si non trouvé

            existingPost.Title = post.Title; // Met à jour le titre
            existingPost.Content = post.Content; // Met à jour le contenu
            existingPost.Upvotes = post.Upvotes; // Met à jour les upvotes
            existingPost.Downvotes = post.Downvotes; // Met à jour les downvotes
            existingPost.Language = post.Language;
            existingPost.Status = post.Status;
            existingPost.Views = post.Views;
            existingPost.CreatedAt = post.CreatedAt;

            // SQL équivalent : SELECT * FROM Users WHERE Id = {post.UserId};
            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId); // Vérifie que l'utilisateur existe
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found."); // 400 si l'utilisateur n'existe pas
            existingPost.User = user;

            // SQL équivalent : UPDATE Posts SET Title='{title}', Content='{content}', Upvotes={upvotes}, Downvotes={downvotes}, Language='{language}', Status='{status}', Views={views} WHERE Id = {id};
            _context.SaveChanges(); // Sauvegarde les changements
            return NoContent(); // 204 si tout s'est bien passé
        }

        // Ajoute un upvote à un post
        // Route: POST api/post/{id}/upvote
        [HttpPost("{id:int}/upvote")]
        [Authorize]
        public IActionResult UpvotePost(int id)
        {
            try
            {
                Console.WriteLine($"Upvote request for post {id}");
                var currentUserIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"Current user ID from token: {currentUserIdString}");
                
                if (string.IsNullOrEmpty(currentUserIdString) || !int.TryParse(currentUserIdString, out int currentUserId))
                {
                    Console.WriteLine("Invalid user token - could not parse user ID");
                    return Unauthorized("Invalid user token.");
                }

                var post = _context.Posts.FirstOrDefault(p => p.Id == id);
                if (post == null)
                {
                    Console.WriteLine($"Post {id} not found");
                    return NotFound($"Post with ID {id} not found.");
                }

                Console.WriteLine($"Found post {id} with current votes - Upvotes: {post.Upvotes}, Downvotes: {post.Downvotes}");

                // Vérifier si l'utilisateur a déjà voté
                var existingVote = _context.PostVotes.FirstOrDefault(v => v.PostId == id && v.UserId == currentUserId);
                if (existingVote != null)
                {
                    Console.WriteLine($"Existing vote found for user {currentUserId} on post {id} - IsUpvote: {existingVote.IsUpvote}");
                    if (existingVote.IsUpvote)
                    {
                        Console.WriteLine("User already upvoted this post");
                        return BadRequest("You have already upvoted this post.");
                    }
                    
                    // Si c'était un downvote, on le change en upvote
                    Console.WriteLine("Changing downvote to upvote");
                    existingVote.IsUpvote = true;
                    post.Downvotes--;
                    post.Upvotes++;
                }
                else
                {
                    // Créer un nouveau vote
                    Console.WriteLine($"Creating new upvote for user {currentUserId} on post {id}");
                    var vote = new PostVote
                    {
                        PostId = id,
                        UserId = currentUserId,
                        IsUpvote = true
                    };
                    _context.PostVotes.Add(vote);
                    post.Upvotes++;
                }

                Console.WriteLine("Saving changes to database");
                _context.SaveChanges();
                Console.WriteLine($"Vote successful - New counts - Upvotes: {post.Upvotes}, Downvotes: {post.Downvotes}");
                return Ok(new { post.Id, post.Upvotes, post.Downvotes });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpvotePost: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Ajoute un downvote à un post
        // Route: POST api/post/{id}/downvote
        [HttpPost("{id:int}/downvote")]
        [Authorize]
        public IActionResult DownvotePost(int id)
        {
            try
            {
                Console.WriteLine($"Downvote request for post {id}");
                var currentUserIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"Current user ID from token: {currentUserIdString}");
                
                if (string.IsNullOrEmpty(currentUserIdString) || !int.TryParse(currentUserIdString, out int currentUserId))
                {
                    Console.WriteLine("Invalid user token - could not parse user ID");
                    return Unauthorized("Invalid user token.");
                }

                var post = _context.Posts.FirstOrDefault(p => p.Id == id);
                if (post == null)
                {
                    Console.WriteLine($"Post {id} not found");
                    return NotFound($"Post with ID {id} not found.");
                }

                Console.WriteLine($"Found post {id} with current votes - Upvotes: {post.Upvotes}, Downvotes: {post.Downvotes}");

                // Vérifier si l'utilisateur a déjà voté
                var existingVote = _context.PostVotes.FirstOrDefault(v => v.PostId == id && v.UserId == currentUserId);
                if (existingVote != null)
                {
                    Console.WriteLine($"Existing vote found for user {currentUserId} on post {id} - IsUpvote: {existingVote.IsUpvote}");
                    if (!existingVote.IsUpvote)
                    {
                        Console.WriteLine("User already downvoted this post");
                        return BadRequest("You have already downvoted this post.");
                    }
                    
                    // Si c'était un upvote, on le change en downvote
                    Console.WriteLine("Changing upvote to downvote");
                    existingVote.IsUpvote = false;
                    post.Upvotes--;
                    post.Downvotes++;
                }
                else
                {
                    // Créer un nouveau vote
                    Console.WriteLine($"Creating new downvote for user {currentUserId} on post {id}");
                    var vote = new PostVote
                    {
                        PostId = id,
                        UserId = currentUserId,
                        IsUpvote = false
                    };
                    _context.PostVotes.Add(vote);
                    post.Downvotes++;
                }

                Console.WriteLine("Saving changes to database");
                _context.SaveChanges();
                Console.WriteLine($"Vote successful - New counts - Upvotes: {post.Upvotes}, Downvotes: {post.Downvotes}");
                return Ok(new { post.Id, post.Upvotes, post.Downvotes });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DownvotePost: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Supprime un post par son id
        // Route: DELETE api/post/{id}
        [HttpDelete("{id:int}")]
        [Authorize] // Require authentication
        public IActionResult DeletePost(int id)
        {
            // Get current user ID from JWT token
            var currentUserIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdString) || !int.TryParse(currentUserIdString, out int currentUserId))
            {
                return Unauthorized("Invalid user token.");
            }

            // SQL équivalent : SELECT * FROM Posts WHERE Id = {id};
            var post = _context.Posts.FirstOrDefault(p => p.Id == id); // Cherche le post
            if (post == null)
                return NotFound($"Post with ID {id} not found."); // 404 si non trouvé

            // Check if the current user owns the post
            if (post.UserId != currentUserId)
            {
                return Forbid("You can only delete your own posts.");
            }

            // SQL équivalent : DELETE FROM Posts WHERE Id = {id};
            _context.Posts.Remove(post); // Supprime le post
            _context.SaveChanges(); // Sauvegarde
            return NoContent(); // 204 si suppression réussie
        }
    }
}