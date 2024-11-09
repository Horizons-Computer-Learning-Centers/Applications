using Horizons.Core.Auth.Service.Interface;
using Horizons.Core.MailService;

namespace Horizons.Core.Auth.Service
{
    public class MailSender : IMailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await EmailSender.SendEmailAsync(email, subject, message);
        }
    }
}
