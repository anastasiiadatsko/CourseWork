using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var fromEmail = _configuration["Smtp:FromEmail"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                using (var message = new MailMessage(fromEmail, email))
                {
                    message.Subject = subject;
                    message.Body = htmlMessage;
                    message.IsBodyHtml = true;
                    try
                    {
                        _logger.LogInformation("SMTP FROM: " + fromEmail);
                        _logger.LogInformation("SMTP TO: " + email);
                        _logger.LogInformation("SMTP HOST: " + smtpHost + ":" + smtpPort);
                        _logger.LogInformation("SMTP USER: " + smtpUser);
                        _logger.LogInformation("SMTP PASS: " + smtpPass.Substring(0, 4) + "****");
                        await client.SendMailAsync(message);
                        _logger.LogInformation("✅ Лист успішно надіслано");
                    }
                    catch (SmtpException ex)
                    {
                        _logger.LogError("❌ SMTP Exception: " + ex.Message);
                        _logger.LogError("❌ StackTrace: " + ex.StackTrace);
                        throw;
                    }
                }
            }
        }
    }
}
