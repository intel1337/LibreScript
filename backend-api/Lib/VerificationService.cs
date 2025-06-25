using MonApiBackend.Models.Context;
using MonApiBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace MonApiBackend.Lib
{
    public interface IVerificationService
    {
        Task<string> GenerateVerificationCodeAsync(int userId);
        Task<bool> VerifyCodeAsync(int userId, string code);
        Task<bool> IsUserVerifiedAsync(int userId);
        Task MarkUserAsVerifiedAsync(int userId);
        Task SendVerificationEmailAsync(User user, string code);
    }

    public class VerificationService : IVerificationService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public VerificationService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> GenerateVerificationCodeAsync(int userId)
        {
            // Générer un code à 6 chiffres
            var code = GenerateRandomCode();
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }

            // Stocker le code et sa date d'expiration (15 minutes)
            user.VerificationCode = code;
            user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            
            await _context.SaveChangesAsync();
            
            return code;
        }

        public async Task<bool> VerifyCodeAsync(int userId, string code)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Vérifier si le code existe, correspond et n'est pas expiré
            if (string.IsNullOrEmpty(user.VerificationCode) ||
                user.VerificationCode != code ||
                user.VerificationCodeExpiry == null ||
                user.VerificationCodeExpiry < DateTime.UtcNow)
            {
                return false;
            }

            // Marquer l'utilisateur comme vérifié et nettoyer le code
            user.Verified = true;
            user.VerificationCode = null;
            user.VerificationCodeExpiry = null;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> IsUserVerifiedAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Verified ?? false;
        }

        public async Task MarkUserAsVerifiedAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Verified = true;
                user.VerificationCode = null;
                user.VerificationCodeExpiry = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendVerificationEmailAsync(User user, string code)
        {
            var subject = "Nouveau code de vérification - LibreScript";
            var body = $@"
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
      <h2>Nouveau code de vérification</h2>
      <p>Bonjour <strong>{user.FullName}</strong>,</p>
      <p>Votre nouveau code de vérification est :</p>
      <p style='text-align: center;'><strong>Votre code de vérification est : <h3>{code}</h3></strong></p>
      <p>Veuillez entrer ce code dans l'application pour vérifier votre compte.</p>
      <p><strong>Ce code expire dans 15 minutes.</strong></p>
      <a href='https://127.0.0.1:5173/verify'>Vérifier mon compte</a>
      <p>Si vous n'avez pas demandé ce nouveau code, ignorez cet email.</p>
      <p>Cordialement,<br>L'équipe LibreScript</p>
      <div class='footer'>
        <p>Ceci est un mail automatique, merci de ne pas y répondre.</p>
      </div>
    </div>
  </body>
</html>";

            try
            {
                var mailService = new SmtpMail(
                    _configuration,
                    "smtp.gmail.com",
                    "noreply@librescript.com",
                    user.Email,
                    subject,
                    body,
                    $"verification-{user.Id}"
                );

                await mailService.SendMailAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'email de vérification: {ex.Message}");
                throw;
            }
        }

        private string GenerateRandomCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var randomNumber = Math.Abs(BitConverter.ToInt32(bytes, 0));
            return (randomNumber % 1000000).ToString("D6");
        }
    }
} 