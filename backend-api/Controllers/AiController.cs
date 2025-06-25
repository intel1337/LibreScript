using Mistral.SDK;
using Microsoft.AspNetCore.Mvc; // import du mvc pour les contrôleurs
using MonApiBackend.Models.Entities; // Import des entités métier
using MonApiBackend.Models.Context; // Import du contexte de la db
using System.Linq; // Import des fonctions linq pour les requêtes
using Microsoft.EntityFrameworkCore; // Import de l'orm de Efc
using System.Threading.Tasks; // Import des tâches asynchrones
using System.Collections.Generic; // Import async pour await
using Mistral.SDK.DTOs;



namespace MonApiBackend.Controllers
{
    [ApiController] // Indique que cette classe est un contrôleur API
    [Route("api/[controller]")] // Définit la route de base pour ce contrôleur en loccurence "api/ai"
    public class AIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MistralClient _client;

        public AIController(AppDbContext context)
        {
            _context = context;
            _client = new MistralClient("r7SoTOCKXSJ795KEzY357OWlvSz6ePuP");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "AIController fonctionne!", timestamp = DateTime.Now });
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string question)
        {
            try
            {
                Console.WriteLine($"Question reçue: {question}");
                
                if (string.IsNullOrEmpty(question))
                {
                    return BadRequest(new { error = "La question ne peut pas être vide" });
                }

                Console.WriteLine("Appel à l'API Mistral...");
                var request = new ChatCompletionRequest(
                    "open-mistral-7b", // Utilisons un modèle public disponible
                    new List<ChatMessage>
                    {
                        new ChatMessage(ChatMessage.RoleEnum.User, question)
                    }
                );

                var response = await _client.Completions.GetCompletionAsync(request);

                Console.WriteLine("Réponse reçue de Mistral");
                var answer = response.Choices.FirstOrDefault()?.Message.Content;
                
                if (string.IsNullOrEmpty(answer))
                {
                    return Ok(new { answer = "Désolé, je n'ai pas pu générer une réponse." });
                }

                return Ok(new { answer = answer });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans AIController: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = $"Erreur interne: {ex.Message}" });
            }
        }
    }
}
