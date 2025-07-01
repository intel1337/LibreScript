using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json; // Pour JsonElement
using Microsoft.Extensions.Configuration; // Ajout pour IConfiguration
using Microsoft.EntityFrameworkCore; // Pour async operations

using System;

namespace MonApiBackend.Controllers
{
    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
    }

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
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest("User data is required.");
            if (_context.Users.Any(u => u.Username == request.Username))
                return BadRequest("Username already exists.");
            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest("Email already exists.");
            
            // Générer le code de vérification à 6 chiffres
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // S'assurer que c'est bien 6 chiffres
            string verificationCode = randomNumber.ToString();
            
            var user = new User
            {
                Username = request.Username,
                FullName = request.FullName,
                Password = request.Password,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                Verified = false,
                VerificationCode = verificationCode,
                VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15) // Code expire dans 15 minutes
            };
            
            _context.Users.Add(user);
            _context.SaveChanges();


            try 
            {
                var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
               var welcomeSubject = $@"Bienvenue {user.FullName} sur LibreScript !";
               var welcomeBody = $@"

<!DOCTYPE html>
<html lang='fr'>
  <head>
    <meta charset='UTF-8'>
    <style>
      body {{
        font-family: Arial, sans-serif;
        color: #333;
        background-color: #f9f9f9;
        padding: 20px;
      }}
      .container {{
        background-color: #ffffff;
        border-radius: 8px;
        padding: 20px;
        max-width: 600px;
        margin: auto;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
      }}
      .footer {{
        margin-top: 30px;
        font-size: 12px;
        color: #777;
      }}
    </style>
  </head>
  <body>
    <div class='container'>
      <h2>Bienvenue sur LibreScript !</h2>
      <p>Bonjour <strong>{user.FullName}</strong>,</p>

      <p>Votre compte a été créé avec succès. Voici les informations liées à votre inscription :</p>
      <p style='text-align: center;'><strong>Votre code de vérification est : <h3>{verificationCode}</h3></strong></p>
      <p>Veuillez entrer ce code dans l'application pour vérifier votre compte.</p>
      <p><strong>Ce code expire dans 15 minutes.</strong></p>
      <a href='https://127.0.0.1:5173/verify'>Vérifier mon compte</a>

      <ul>
        <li><strong>Nom d'utilisateur :</strong> {user.Username}</li>
        <li><strong>Email :</strong> {user.Email}</li>
        <li><strong>Date de création :</strong> {user.CreatedAt:dd/MM/yyyy}</li>
      </ul>

      <p>Merci de nous avoir rejoint !</p>

      <p>Cordialement,<br>L'équipe LibreScript</p>

      <div class='footer'>
        <p>Ceci est un mail automatique, merci de ne pas y répondre.</p>
      </div>
    </div>
  </body>
</html>";


                var mailService = new MonApiBackend.Lib.SmtpMail(
                    configuration,
                    "smtp.gmail.com",           // SMTP host
                    "noreply@librescript.com",  // From (ou votre email)
                    user.Email,                 // To (email de l'utilisateur)
                    welcomeSubject,             // Subject
                    welcomeBody,                // Body
                    $"registration-{user.Id}"   // UserState pour le tracking
                );

                _ = Task.Run(async () => await mailService.SendMailAsync());
            }
            catch (Exception emailEx)
            {
                Console.WriteLine($"Failed to send welcome email: {emailEx.Message}");
            }

            // Génération du JWT (votre code existant)
            var jwtKey = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt:Key").Value;
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("UserId", user.Id.ToString()), // Ajout explicite pour le VerificationController
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
        // Route: POST api/user/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
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
                    new Claim("UserId", user.Id.ToString()), // Ajout explicite pour le VerificationController
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
             
                Console.WriteLine("Token claims:");
                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                }

                try
                {
                    var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;
                    var username = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
                    var email = jwtToken.Claims.First(x => x.Type == "email").Value;
                    return Ok(new { userId, username, email });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error finding claims: {ex.Message}");
                    return Unauthorized($"Error finding claims: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid or expired token: {ex.Message}");
            }
        }

        // Get current user profile
        // Route: GET api/user/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                // Return user profile (without password)
                return Ok(new {
                    id = user.Id,
                    username = user.Username,
                    fullName = user.FullName,
                    email = user.Email,
                    verified = user.Verified,
                    createdAt = user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid token: {ex.Message}");
            }
        }

        // Update user profile
        // Route: PUT api/user/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] JsonElement profileData)
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound("User not found");

                // Update user fields
                if (profileData.TryGetProperty("fullName", out var fullNameProp))
                    user.FullName = fullNameProp.GetString();
                
                if (profileData.TryGetProperty("email", out var emailProp))
                {
                    var newEmail = emailProp.GetString();
                    // Check if email is already taken by another user
                    if (_context.Users.Any(u => u.Email == newEmail && u.Id != userId))
                        return BadRequest("Email already exists");
                    user.Email = newEmail;
                }

                if (profileData.TryGetProperty("password", out var passwordProp))
                {
                    var newPassword = passwordProp.GetString();
                    if (!string.IsNullOrEmpty(newPassword))
                        user.Password = newPassword;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid token: {ex.Message}");
            }
        }

        // Get user's posts
        // Route: GET api/user/posts
        [HttpGet("posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            Console.WriteLine("GetUserPosts endpoint called");
            
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                Console.WriteLine("Missing or invalid Authorization header");
                return Unauthorized("Missing or invalid Authorization header.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            Console.WriteLine($"Token received: {token.Substring(0, Math.Min(20, token.Length))}...");
            
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
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
                var username = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
                Console.WriteLine($"User ID from token: {userId}");
                Console.WriteLine($"Username from token: {username}");

                // Debug: Check all posts in database
                var allPosts = await _context.Posts.Include(p => p.User).ToListAsync();
                Console.WriteLine($"Total posts in database: {allPosts.Count}");
                foreach (var post in allPosts)
                {
                    Console.WriteLine($"Post ID: {post.Id}, Title: {post.Title}, UserId: {post.UserId}, Author: {post.User?.Username}");
                }

                var userPosts = await _context.Posts
                    .Include(p => p.User)
                    .Where(p => p.User.Username == username)
                    .Select(p => new {
                        p.Id,
                        p.Title,
                        p.Content,
                        p.Language,
                        p.Status,
                        p.Upvotes,
                        p.Downvotes,
                        p.Views,
                        p.CreatedAt,
                        Author = new {
                            p.User.Id,
                            p.User.Username,
                            p.User.FullName
                        },
                        CommentsCount = _context.Comments.Where(c => c.PostId == p.Id).Count()
                    })
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                Console.WriteLine($"Found {userPosts.Count} posts for user {userId}");
                
                return Ok(userPosts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserPosts: {ex.Message}");
                return Unauthorized($"Invalid token: {ex.Message}");
            }
        }

        // Debug endpoint to see all users and posts
        // Route: GET api/user/debug
        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                var posts = await _context.Posts.Include(p => p.User).ToListAsync();

                return Ok(new { 
                    users = users.Select(u => new { u.Id, u.Username, u.FullName, u.Email }),
                    posts = posts.Select(p => new { 
                        p.Id, 
                        p.Title, 
                        p.UserId, 
                        AuthorUsername = p.User?.Username,
                        p.CreatedAt 
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Debug error: {ex.Message}");
            }
        }

        // Test endpoint to verify JWT configuration
        // Route: GET api/user/test-jwt
        [HttpGet("test-jwt")]
        public IActionResult TestJwt()
        {
            try
            {
                var jwtKey = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Jwt:Key").Value;
                if (string.IsNullOrEmpty(jwtKey))
                {
                    return BadRequest("JWT Key not found in configuration");
                }
                return Ok(new { 
                    message = "JWT configuration is working", 
                    keyLength = jwtKey.Length,
                    keyExists = !string.IsNullOrEmpty(jwtKey)
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"JWT configuration error: {ex.Message}");
            }
        }
    }
}