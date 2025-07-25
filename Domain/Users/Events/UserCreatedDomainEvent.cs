using Domain.Abstraction;

namespace Domain.Users.Events;
public record UserCreatedDomainEvent(Guid UserId) : ISuccDomainEvent
{

}
