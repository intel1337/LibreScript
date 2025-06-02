using Microsoft.AspNetCore.Http; // Import pour la gestion des requêtes HTTP
using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier

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
            var posts = _context.Posts.ToList(); // Récupère tous les posts
            return Ok(posts); // Retourne la liste avec un code 200 OK
        }



        // Renvoie un post par son id
        // Route: GET api/post/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetPostById(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id); // Cherche le post par id
            if (post == null)
                return NotFound($"Post with ID {id} not found."); // 404 si non trouvé
            return Ok(post); // Retourne le post avec un code 200 OK
        }



        // Crée un nouveau post Route de base en POST
        // Route: POST api/post
        [HttpPost]
        public IActionResult CreatePost([FromBody] Post post)
        {
            if (post == null)
                return BadRequest("Post data is required."); // 400 si aucune donnée reçue

            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId); // Vérifie que l'utilisateur existe
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found."); // 400 si l'utilisateur n'existe pas
            post.User = user;

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

            var existingPost = _context.Posts.FirstOrDefault(p => p.Id == id); // Cherche le post existant
            if (existingPost == null)
                return NotFound($"Post with ID {id} not found."); // 404 si non trouvé

            existingPost.Title = post.Title; // Met à jour le titre
            existingPost.Content = post.Content; // Met à jour le contenu

            var user = _context.Users.FirstOrDefault(u => u.Id == post.UserId); // Vérifie que l'utilisateur existe
            if (user == null)
                return BadRequest($"User with ID {post.UserId} not found."); // 400 si l'utilisateur n'existe pas
            existingPost.User = user;

            _context.SaveChanges(); // Sauvegarde les changements
            return NoContent(); // 204 si tout s'est bien passé
        }



        // Supprime un post par son id
        // Route: DELETE api/post/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id); // Cherche le post
            if (post == null)
                return NotFound($"Post with ID {id} not found."); // 404 si non trouvé

            _context.Posts.Remove(post); // Supprime le post
            _context.SaveChanges(); // Sauvegarde
            return NoContent(); // 204 si suppression réussie
        }
    }
}