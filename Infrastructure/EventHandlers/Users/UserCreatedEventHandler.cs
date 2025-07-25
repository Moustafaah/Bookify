using Domain.Abstraction;
using Domain.Users.Events;

namespace Infrastructure.EventHandlers.Users;
internal class UserCreatedEventHandler : IEventHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent evt)
    {
        Console.WriteLine($"Event {nameof(UserCreatedDomainEvent)}: {evt.UserId} got handled");
        return Task.CompletedTask;
    }
}
