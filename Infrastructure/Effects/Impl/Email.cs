using Infrastructure.Effects.Traits;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Effects.Impl;

public class Email : IEmail
{
    public static IEmail Default => new Email();
    public async Task<Response> Send(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent, string apiKey, CancellationToken token)
    {
        var client = new SendGridClient(apiKey);
        var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return await client.SendEmailAsync(message, token);

    }
}