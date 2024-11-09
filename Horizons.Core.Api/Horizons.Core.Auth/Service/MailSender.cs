using Azure.Communication.Email;
using Azure;
using Horizons.Core.Auth.Service.Interface;


namespace Horizons.Core.Auth.Service
{
    public class MailSender : IMailSender
    {
        private readonly IConfiguration _configuration;

        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string connectionString = _configuration.GetConnectionString("CommunicationServices");
            var emailClient = new EmailClient(connectionString);

            var emailMessage = new EmailMessage(
                senderAddress: "DoNotReply@horizon-centers.com",
                content: new EmailContent("Test Email")
                {
                    PlainText = "Hello world via email.",
                    Html = $@"
		            <html>
			            <body>
				            <h1>{subject}</h1>
                             <p>{message}</p>
			            </body>
		            </html>"
                },
                recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(email) }));


            EmailSendOperation emailSendOperation = emailClient.Send(WaitUntil.Completed, emailMessage);
        }
    }
}
