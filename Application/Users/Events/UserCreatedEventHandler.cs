using Application.Users.Events.HtmlTemplates;
using Application.Users.Queries;

using HandlebarsDotNet;

using Infrastructure.Abstractions;
using Infrastructure.Effects;
using Infrastructure.Effects.FileSystem;
using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;

namespace Application.Users.Events;

internal class UserCreatedEventHandler : IEventHandler<UserCreatedDomainEvent>
{

    public K<M, Unit> Handle<M, RT>(UserCreatedDomainEvent evt) where M : MonadIO<M>, Fallible<M> where RT : Has<M, IEmail>, Has<M, IDatabase>, Has<M, Config>
    {
        const string sql = @"SELECT
                                    id AS Id,
                                    firstname AS FirstName,
                                    lastname AS LastName,
                                    email AS Email
                                FROM Users
                                WHERE Id = @Id";

        Console.WriteLine("___________________________________");

        return

            from u in Database<M, RT>.QuerySingle<UserViewModel>(sql, new { id = evt.UserId })
            let props = new WelcomeEmailProps(u.Email, u.FirstName)
            let template = HandlebarTemplateHelpers.GetWelcomeTemplate.Value(props)
            //from _ in Email<M, RT>.Send(new EmailAddress("MoustafaAhmed@yahoo.com"), new EmailAddress(u.Email, $"{u.FirstName} {u.LastName}"), "Welcome on board", "we welcome u", template)
            from a in M.LiftIO(IO.lift(() => Console.WriteLine($"User Created And Handled")))
            select unit;

    }



}

public static class HandlebarTemplateHelpers
{
    public static Lazy<Func<object?, string>> GetWelcomeTemplate = new Lazy<Func<object?, string>>(() =>
    {
        string basePath = AppContext.BaseDirectory;
        string templatePath = Path.Combine(
            basePath,
            "Users",
            "Events",
            "HtmlTemplates",
            "WelcomeEmail.hbs"
        );
        var text = File.ReadAllText(templatePath);
        var compiled = Handlebars.Compile(text);
        return compiled;
    });


    private static string GetTemplate(object? data)
    {
        string basePath = AppContext.BaseDirectory;
        string templatePath = Path.Combine(
            basePath,
            "Users",
            "Events",
            "HtmlTemplates",
            "WelcomeEmail.hbs"
        );

        Console.WriteLine("################## Template compiled''############################");
        return (from template in FileIO<IO>.ReadAllText(templatePath)
                from h in IO.lift(() => Handlebars.Compile(template)(data))
                select h).Run();
    }
}