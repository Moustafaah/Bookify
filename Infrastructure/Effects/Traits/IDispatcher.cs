using Domain.Abstraction;

using LanguageExt.Traits;

namespace Infrastructure.Effects.Traits;
public interface IDispatcher
{
    public K<M, Unit> Dispatch<M, RT>(IDomainEvent domainEvent, IEnumerable<object> handlers)
        where M : MonadIO<M>, Fallible<M>;
}
