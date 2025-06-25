using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace MonApiBackend.Lib
{
    public class SmtpMail
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly string _from;
        private readonly string _to;
        private readonly string _subject;
        private readonly string _body;
        private readonly string _userState;

        public SmtpMail(IConfiguration configuration, string smtpHost, string from, string to, string subject, string body, string userState = "mail")
        {
            _configuration = configuration;
            _smtpHost = smtpHost;
            _from = from;
            _to = to;
            _subject = subject;
            _body = body;
            _userState = userState;
        }

        public async Task<bool> SendMailAsync()
        {
            try
            {
                var gmailSecret = _configuration["GmailSecret"];
                var gmailUser = _configuration["GmailUser"];

                if (string.IsNullOrEmpty(gmailSecret) || string.IsNullOrEmpty(gmailUser))
                {
                    Console.WriteLine("Gmail credentials not found in configuration");
                    return false;
                }

                // Create SMTP client
                using var client = new SmtpClient(_smtpHost)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(gmailUser, gmailSecret),
                    EnableSsl = true
                };

                // Create email message
                using var message = new MailMessage(_from, _to)
                {
                    Subject = _subject,
                    Body = _body,
                    IsBodyHtml = true,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8
                };

                // Send email asynchronously
                await client.SendMailAsync(message);
                Console.WriteLine($"[{_userState}] Email sent successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{_userState}] Error sending email: {ex.Message}");
                return false;
            }
        }

        // Synchronous version for backward compatibility
        public bool SendMail()
        {
            return SendMailAsync().GetAwaiter().GetResult();
        }
    }
}
