using Azure;
using Azure.Communication.Email;

namespace Horizons.Core.MailService
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            string? connectionString = Environment.GetEnvironmentVariable("COMMUNICATION_SERVICES_CONNECTION_STRING");
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
