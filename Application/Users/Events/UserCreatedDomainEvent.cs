using Domain.Abstraction;

namespace Application.Users.Events;
public record UserCreatedDomainEvent(Guid UserId) : ISuccDomainEvent
{

}
