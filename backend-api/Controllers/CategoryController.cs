using Microsoft.AspNetCore.Mvc; // import du mvc pour les contrôleurs
using MonApiBackend.Models.Entities; // Import des entités métier
using MonApiBackend.Models.Context; // Import du contexte de la db
using System.Linq; // Import des fonctions linq pour les requêtes
using Microsoft.EntityFrameworkCore; // Import de l'orm de Efc
using System.Threading.Tasks; // Import des tâches asynchrones
using System.Collections.Generic; // Import async pour await 

namespace MonApiBackend.Controllers
{
    [ApiController] // Indique que cette classe est un contrôleur API
    [Route("api/[controller]")] // Définit la route de base pour ce contrôleur en loccurence "api/category"
    public class CategoryController : ControllerBase
    {
        // Instance privée du contexte de la base de données (hérite de DbContext)
        private readonly AppDbContext _context;

        // Constructeur du contrôleur, injection du contexte de la base de données
        public CategoryController(AppDbContext context)
        {
            _context = context; // _ pour indiquer une variable privée
        }



        // Renvoie toutes les catégories
        // Route: GET api/category
        [HttpGet] // précise pas la route car c'est la route de base
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            // SQL équivalent : SELECT * FROM Categories;
            var categories = await _context.Categories.ToListAsync(); // Récupère toutes les catégories
            return Ok(categories); // Retourne la liste avec un code 200 OK
        }



        // Renvoie une catégorie par son id
        // Route: GET api/category/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            // SQL équivalent : SELECT * FROM Categories WHERE Id = {id};
            var category = await _context.Categories.FindAsync(id); // Cherche la catégorie par id
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found."); // 404 si non trouvée
            }
            return Ok(category); // Retourne la catégorie avec un code 200 OK
        }



        // Crée une nouvelle catégorie
        // Route: POST api/category
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required."); // 400 si aucune donnée reçue
            }

            // SQL équivalent : INSERT INTO Categories (Name, Description) VALUES ('{name}', '{description}');
            _context.Categories.Add(category); // Ajoute la catégorie à la base
            await _context.SaveChangesAsync(); // Sauvegarde les changements

            // Retourne 201 Created avec l'URL de la nouvelle catégorie
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }



        // Met à jour une catégorie existante
        // Route: PUT api/category/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("Category ID not matching."); // 400 si l'id ne correspond pas
            }

            // SQL équivalent : UPDATE Categories SET Name='{name}', Description='{description}' WHERE Id = {id};
            _context.Entry(category).State = EntityState.Modified; // Marque la catégorie comme modifiée puis SaveChangesAsync sera appelée plus tard

            try
            {
                await _context.SaveChangesAsync(); // Tente de sauvegarder 
            }
            catch (DbUpdateConcurrencyException)
            {
                // SQL équivalent : SELECT COUNT(*) FROM Categories WHERE Id = {id};
                if (!_context.Categories.Any(e => e.Id == id)) // Vérifie si la catégorie existe
                {
                    return NotFound($"Category with ID {id} not found."); // 404 si la catégorie n'existe pas
                }
                else
                {
                    throw; // Relance l'exception si autre problème
                }
            }

            return NoContent(); // 204 si tout s'est bien passé
        }



        // Supprime une catégorie par son id
        // Route: DELETE api/category/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // SQL équivalent : SELECT * FROM Categories WHERE Id = {id};
            var category = await _context.Categories.FindAsync(id); // Cherche la catégorie
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found."); // 404 si non trouvée
            }

            // SQL équivalent : DELETE FROM Categories WHERE Id = {id};
            _context.Categories.Remove(category); // Supprime la catégorie
            await _context.SaveChangesAsync(); // Sauvegarde

            return NoContent(); // 204 si suppression réussie
        }
    }
}