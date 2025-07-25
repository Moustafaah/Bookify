namespace Domain.Abstraction;
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent evt);
}

