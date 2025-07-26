using Domain.Abstraction;
using Domain.Users.Events;

using Infrastructure.Effects;
using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;

using SendGrid.Helpers.Mail;

namespace Infrastructure.EventHandlers.Users;
internal class UserCreatedEventHandler : IEventHandler<UserCreatedDomainEvent>
{
    public K<M, Unit> Handle<M, RT>(UserCreatedDomainEvent evt) where M : MonadIO<M>, Fallible<M> where RT : Has<M, IEmail>, Has<M, Config>
    {
        return from x in Email<M, RT>.Send(
               new EmailAddress("mustafaamaar@yahoo.com", "Moustafa Ahmed"),
               new EmailAddress(evt.User.Email.Repr, $" {evt.User.Firstname.Repr}"), "Congrats for registration",
               "Hallo Bro", "<h1>HalloDear</h1>")
               select x;

    }
}
