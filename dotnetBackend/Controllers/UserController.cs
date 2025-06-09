using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration; // Ajout pour IConfiguration

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
            // SQL équivalent : SELECT * FROM Users WHERE Id = {id};
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
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User data is required.");
            // SQL équivalent : SELECT COUNT(*) FROM Users WHERE Username = '{username}';
            if (_context.Users.Any(u => u.Username == user.Username))
                return BadRequest("Username already exists.");
            // SQL équivalent : SELECT COUNT(*) FROM Users WHERE Email = '{email}';
            if (_context.Users.Any(u => u.Email == user.Email))
                return BadRequest("Email already exists.");
            user.CreatedAt = DateTime.UtcNow;
            // SQL équivalent : INSERT INTO Users (Username, Email, Password, CreatedAt) VALUES ('{username}', '{email}', '{password}', '{createdAt}');
            _context.Users.Add(user);
            _context.SaveChanges();

            // Récupération de la clé secrète depuis la config
            var jwtKey = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt:Key").Value;
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { token = jwt });
        }

        // Connexion d'un utilisateur (retourne un JWT si ok)
        // Route: POST api/user/login/
        [HttpPost("login/")]
        public IActionResult Login([FromBody] User login)
        {
            if (login == null)
                return BadRequest("User data is required.");
            // SQL équivalent : SELECT * FROM Users WHERE Username = '{username}';
            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);
            if (user == null || user.Password != login.Password)
                return Unauthorized("Invalid username or password.");
            // Génération du JWT
            var jwtKey = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt:Key").Value;
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return Ok(new { token = jwt });
        }

        // Vérifie le JWT envoyé par le front (depuis localStorage)
        // Route: POST api/user/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Missing or invalid Authorization header.");
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var jwtKey = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt:Key").Value;
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                var email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                return Ok(new { userId, username, email });
            }
            catch
            {
                return Unauthorized("Invalid or expired token.");
            }
        }
    }
}