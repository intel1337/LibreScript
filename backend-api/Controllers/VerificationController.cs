using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MonApiBackend.Lib;
using MonApiBackend.Models.Context;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MonApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IVerificationService _verificationService;

        public VerificationController(AppDbContext context, IVerificationService verificationService)
        {
            _context = context;
            _verificationService = verificationService;
        }

        [HttpGet("status")]
        [Authorize]
        public async Task<IActionResult> GetVerificationStatus()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "Token invalide" });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { error = "Utilisateur non trouvé" });
                }

                if (user.Verified)
                {
                    return Ok(new { verified = true, message = "Votre compte est déjà vérifié." });
                }

                return Ok(new { verified = false, message = "Votre compte n'est pas vérifié. Veuillez entrer le code de vérification envoyé par email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur interne: {ex.Message}" });
            }
        }

        [HttpPost("send-code")]
        [Authorize]
        public async Task<IActionResult> SendVerificationCode()
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "Token invalide" });
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new { error = "Utilisateur non trouvé" });
                }

                if (user.Verified)
                {
                    return BadRequest(new { error = "Votre compte est déjà vérifié." });
                }

                // Générer et envoyer le code
                var code = await _verificationService.GenerateVerificationCodeAsync(userId);
                await _verificationService.SendVerificationEmailAsync(user, code);

                return Ok(new { message = "Code de vérification envoyé par email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur lors de l'envoi du code: {ex.Message}" });
            }
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "Token invalide" });
                }

                if (string.IsNullOrEmpty(request.Code) || request.Code.Length != 6)
                {
                    return BadRequest(new { error = "Le code doit contenir exactement 6 chiffres." });
                }

                var isValid = await _verificationService.VerifyCodeAsync(userId, request.Code);
                
                if (isValid)
                {
                    return Ok(new { verified = true, message = "Votre compte a été vérifié avec succès!" });
                }
                else
                {
                    return BadRequest(new { error = "Code de vérification invalide ou expiré." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur lors de la vérification: {ex.Message}" });
            }
        }
    }

    public class VerifyCodeRequest
    {
        public string Code { get; set; } = "";
    }
} 