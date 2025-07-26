using Domain.Abstraction;

namespace Domain.Users.Events;
public record UserCreatedDomainEvent(User User) : ISuccDomainEvent
{

}
