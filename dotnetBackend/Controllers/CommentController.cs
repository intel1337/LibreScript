using Microsoft.AspNetCore.Mvc; // Import du MVC pour les contrôleurs
using MonApiBackend.Models.Context; // Import du contexte de la base de données
using MonApiBackend.Models.Entities; // Import des entités métier
using System.Collections.Generic; // Pour les collections génériques
using System.Linq; // Pour les requêtes LINQ
using System.Threading.Tasks; // Pour l'asynchrone
using Microsoft.EntityFrameworkCore; // Pour l'ORM Entity Framework Core

namespace MonApiBackend.Controllers
{
    [ApiController] // Indique que cette classe est un contrôleur API
    [Route("api/[controller]")] // Définit la route de base pour ce contrôleur, ici "api/comment"
    public class CommentController : ControllerBase
    {
        // Instance privée du contexte de la base de données (hérite de DbContext)
        private readonly AppDbContext _context;

        // Constructeur du contrôleur, injection du contexte de la base de données
        public CommentController(AppDbContext context)
        {
            _context = context;
        }



        // Renvoie tous les commentaires
        // Route: GET api/comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            // SQL équivalent : SELECT * FROM Comments;
            // Récupère tous les commentaires
            return await _context.Comments.ToListAsync();
        }



        // Renvoie un commentaire par son id
        // Route: GET api/comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            // SQL équivalent : SELECT * FROM Comments WHERE Id = {id};
            var comment = await _context.Comments.FindAsync(id); // Cherche le commentaire par id
            if (comment == null)
            {
                return NotFound(); // 404 si non trouvé
            }
            return comment; // Retourne le commentaire
        }



        // Crée un nouveau commentaire
        // Route: POST api/comment
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            // Ajoute la date de création
            comment.CreatedAt = System.DateTime.UtcNow;
            // SQL équivalent : INSERT INTO Comments (Content, UserId, PostId, ParentCommentId, CreatedAt) VALUES (...);
            _context.Comments.Add(comment); // Ajoute le commentaire à la base
            await _context.SaveChangesAsync(); // Sauvegarde
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment); // Retourne 201 Created
        }



        // Crée une réponse à un commentaire (reply)
        // Route: POST api/comment/{parentId}/replies
        [HttpPost("{parentId:int}/replies")]
        public async Task<ActionResult<Comment>> PostReply(int parentId, Comment reply)
        {
            // SQL équivalent : SELECT * FROM Comments WHERE Id = {parentId};
            var parentComment = await _context.Comments.FindAsync(parentId); // Cherche le commentaire parent
            if (parentComment == null)
            {
                return NotFound($"Parent comment with ID {parentId} not found."); // 404 si parent non trouvé
            }
            reply.ParentCommentId = parentId; // Lie la réponse au parent
            reply.PostId = parentComment.PostId; // Même post que le parent
            reply.CreatedAt = System.DateTime.UtcNow;
            // SQL équivalent : INSERT INTO Comments (Content, UserId, PostId, ParentCommentId, CreatedAt) VALUES (...);
            _context.Comments.Add(reply); // Ajoute la réponse
            await _context.SaveChangesAsync(); // Sauvegarde
            return CreatedAtAction(nameof(GetComment), new { id = reply.Id }, reply); // Retourne 201 Created
        }



        // Renvoie toutes les réponses à un commentaire
        // Route: GET api/comment/{parentId}/replies
        [HttpGet("{parentId:int}/replies")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetRepliesForComment(int parentId)
        {
            // SQL équivalent : SELECT COUNT(*) FROM Comments WHERE Id = {parentId};
            var parentCommentExists = await _context.Comments.AnyAsync(c => c.Id == parentId); // Vérifie que le parent existe
            if (!parentCommentExists)
            {
                return NotFound($"Parent comment with ID {parentId} not found."); // 404 si parent non trouvé
            }
            // SQL équivalent : SELECT * FROM Comments WHERE ParentCommentId = {parentId};
            var replies = await _context.Comments
                                      .Where(c => c.ParentCommentId == parentId)
                                      .ToListAsync(); // Récupère les réponses
            return Ok(replies); // Retourne la liste
        }



        // Met à jour un commentaire existant
        // Route: PUT api/comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest(); // 400 si l'id ne correspond pas
            }
            // SQL équivalent : UPDATE Comments SET Content='{content}', UserId={userId}, PostId={postId}, ParentCommentId={parentCommentId} WHERE Id = {id};
            _context.Entry(comment).State = EntityState.Modified; // Marque comme modifié
            try
            {
                await _context.SaveChangesAsync(); // Sauvegarde
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound(); // 404 si non trouvé
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); // 204 si tout s'est bien passé
        }



        // Supprime un commentaire par son id
        // Route: DELETE api/comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // SQL équivalent : SELECT * FROM Comments WHERE Id = {id};
            var comment = await _context.Comments.FindAsync(id); // Cherche le commentaire
            if (comment == null)
            {
                return NotFound(); // 404 si non trouvé
            }
            // SQL équivalent : DELETE FROM Comments WHERE Id = {id};
            _context.Comments.Remove(comment); // Supprime le commentaire
            await _context.SaveChangesAsync(); // Sauvegarde
            return NoContent(); // 204 si suppression réussie
        }

        // Vérifie si un commentaire existe (méthode privée)
        private bool CommentExists(int id)
        {
            // SQL équivalent : SELECT COUNT(*) FROM Comments WHERE Id = {id};
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}