namespace Horizons.Core.Auth.Service.Interface
{
    public interface IMailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
