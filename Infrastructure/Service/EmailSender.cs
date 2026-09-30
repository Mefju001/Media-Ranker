using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service
{
    internal class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> logger;
        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            logger.LogWarning("--- EMAIL MOCK ---");
            logger.LogWarning("Do: {email} | Temat: {subject}", email, subject);
            logger.LogWarning("Treść: {htmlMessage}", htmlMessage);
            logger.LogWarning("------------------");
            return Task.CompletedTask;
        }
    }
}
