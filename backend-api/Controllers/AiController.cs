using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;

namespace dotnetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AiController> _logger;
        private readonly string _aiApiUrl = "http://localhost:5050"; // URL de l'API Flask

        public AiController(HttpClient httpClient, ILogger<AiController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Vérifier le statut de l'API IA
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetAiStatus()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_aiApiUrl}/status");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<object>(content));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { error = "API IA non disponible" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification du statut de l'API IA");
                return StatusCode(500, new { error = "Erreur de communication avec l'API IA" });
            }
        }

        /// <summary>
        /// Générer une réponse avec l'IA
        /// </summary>
        [HttpPost("generate")]
        [Authorize] // Nécessite une authentification
        public async Task<IActionResult> GenerateResponse([FromBody] AiGenerateRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Prompt))
                {
                    return BadRequest(new { error = "Le prompt est requis" });
                }

                // Préparer la requête pour l'API Flask
                var aiRequest = new
                {
                    prompt = request.Prompt,
                    length = request.Length ?? 200,
                    temperature = request.Temperature ?? 0.7
                };

                var json = JsonSerializer.Serialize(aiRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Envoi de requête à l'API IA: {request.Prompt.Substring(0, Math.Min(50, request.Prompt.Length))}...");

                var response = await _httpClient.PostAsync($"{_aiApiUrl}/generate", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var aiResponse = JsonSerializer.Deserialize<AiGenerateResponse>(responseContent);
                    
                    return Ok(new
                    {
                        success = true,
                        prompt = aiResponse.prompt,
                        response = aiResponse.response,
                        parameters = aiResponse.parameters,
                        timestamp = aiResponse.timestamp
                    });
                }
                else
                {
                    _logger.LogError($"Erreur de l'API IA: {responseContent}");
                    return StatusCode((int)response.StatusCode, JsonSerializer.Deserialize<object>(responseContent));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération de réponse IA");
                return StatusCode(500, new { error = "Erreur interne lors de la génération de réponse" });
            }
        }

        /// <summary>
        /// Recharger le modèle IA
        /// </summary>
        [HttpPost("reload")]
        [Authorize] // Nécessite une authentification
        public async Task<IActionResult> ReloadAiModel()
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_aiApiUrl}/reload", null);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Ok(JsonSerializer.Deserialize<object>(responseContent));
                }
                else
                {
                    return StatusCode((int)response.StatusCode, JsonSerializer.Deserialize<object>(responseContent));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du rechargement du modèle IA");
                return StatusCode(500, new { error = "Erreur lors du rechargement du modèle" });
            }
        }

        /// <summary>
        /// Test de connectivité avec l'API IA
        /// </summary>
        [HttpGet("health")]
        public async Task<IActionResult> CheckAiHealth()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_aiApiUrl}/");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(new 
                    { 
                        connected = true, 
                        ai_status = JsonSerializer.Deserialize<object>(content) 
                    });
                }
                else
                {
                    return Ok(new { connected = false, status_code = (int)response.StatusCode });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur de connectivité avec l'API IA");
                return Ok(new { connected = false, error = ex.Message });
            }
        }
    }

    // Modèles de données
    public class AiGenerateRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public int? Length { get; set; }
        public double? Temperature { get; set; }
    }

    public class AiGenerateResponse
    {
        public bool success { get; set; }
        public string prompt { get; set; } = string.Empty;
        public string response { get; set; } = string.Empty;
        public AiParameters parameters { get; set; } = new();
        public string timestamp { get; set; } = string.Empty;
    }

    public class AiParameters
    {
        public int length { get; set; }
        public double temperature { get; set; }
    }
} 