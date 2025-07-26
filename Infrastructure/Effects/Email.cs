using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;

using SendGrid.Helpers.Mail;

namespace Infrastructure.Effects;

public static class Email<M, RT> where RT :
    Has<M, Config>,
    Has<M, IEmail> where M : Fallible<M>, Monad<M>, MonadIO<M>
{
    private static K<M, IEmail> Trait => Has<M, RT, IEmail>.ask;


    public static K<M, Unit> Send(EmailAddress from, EmailAddress to, string subject, string plainTextContent, string htmlContent)
    {


        return from apiKey in Config<M, RT>.SendGridKey
               from e in Trait
               from r in liftIO(async envIo =>
                   await e.Send(@from, @to, subject, plainTextContent, htmlContent, apiKey, envIo.Token))
               select unit;
    }
}



