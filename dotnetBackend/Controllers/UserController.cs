using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier

namespace MonApiBackend.Controllers
{
    [ApiController] // Indique que cette classe est un contrôleur API
    [Route("api/[controller]")] // Définit la route de base pour ce contrôleur, ici "api/user"
    public class UserController : ControllerBase
    {
        // Instance privée du contexte de la base de données (hérite de DbContext)
        private readonly AppDbContext _context;

        // Constructeur du contrôleur, injection du contexte de la base de données
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Renvoie un utilisateur par son id
        // Route: GET api/user/get-user/id/{id}
        [HttpGet("get-user/id/{id:int}")]
        public IActionResult GetUserById(int id)
        {
            var result = _context.Users.FirstOrDefault(u => u.Id == id); // Cherche l'utilisateur par id
            return Ok(result); // Retourne l'utilisateur (ou null si non trouvé)
        }

        // Renvoie un utilisateur par son username
        // Route: GET api/user/get-user/username/{username}
        [HttpGet("get-user/username/{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            // Ici, la logique n'est pas implémentée, juste un message de test
            return Ok($"Hello from get user with username {username}!");
        }

        // Inscription d'un nouvel utilisateur
        // Route: POST api/user/register/
        [HttpPost("register/")]
        public IActionResult Register()
        {
            // Ici, la logique d'inscription n'est pas implémentée, juste un message de test
            return Ok("Hello from register!");
        }

        // Connexion d'un utilisateur
        // Route: POST api/user/login/
        [HttpPost("login/")]
        public IActionResult Login()
        {
            // Ici, la logique de connexion n'est pas implémentée, juste un message de test
            return Ok("Hello from login!");
        }
    }
}