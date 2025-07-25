using Domain.Abstraction;

namespace Infrastructure.Effects.Traits;
public interface IDispatcher
{
    public Task Dispatch(IDomainEvent domainEvent, IEnumerable<object> handlers);
}
