using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Effects.Traits;

public interface IEmail
{
    public Task<Response> Send(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent, string apiKey, CancellationToken token);
}