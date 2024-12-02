using SendGrid.Helpers.Mail;
using SendGrid;

namespace HomeFoodNetwork.Models
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(string toEmail, string fromEmail, string subject,
                                string content, string htmlContent);
    }

    public class EmailProviderSendGrid : IEmailProvider
    {
        private readonly IConfiguration _config;
        public EmailProviderSendGrid(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string fromEmail, string subject, string content, string htmlContent)
        {
            var apiKey = _config.GetSection("SendGridKey").Value;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("azael0105.s@gmail.com", "Test Email"),
                Subject = "Testing 123",
                PlainTextContent = "This is a test Email",
                HtmlContent = "<strong>This is a test Email</strong>"
            };
            msg.AddTo(new EmailAddress("azael0105.s@gmail.com", "Ian Hue"));
            await client.SendEmailAsync(msg);
            // var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
