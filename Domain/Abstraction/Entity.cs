namespace Domain.Abstraction;
public abstract record Entity : IDomainEvent
{
    private readonly List<IDomainEvent> _domainEvents =
    [];


    protected Entity(Guid id)
    {
        Id = id;
    }


    public Guid Id { get; }

    public Unit RaiseDomainEvent(IDomainEvent @event)
    {

        _domainEvents.Add(@event);
        return unit;
    }

    public Unit ClearDomainEvents()
    {
        _domainEvents.Clear();
        return unit;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList().AsReadOnly();
    }



}
