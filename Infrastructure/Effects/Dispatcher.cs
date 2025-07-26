using Domain.Abstraction;

using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.Effects;
public static class Dispatcher<M, RT> where M : MonadIO<M>, Fallible<M> where RT : Has<M, IDispatcher>
{
    private static K<M, IDispatcher> Trait => Has<M, RT, IDispatcher>.ask;


    public static K<M, Unit> Dispatch(IDomainEvent domainEvent, IEnumerable<object> handlers)
    {
        return from d in Trait
               from _ in d.Dispatch<M>(domainEvent, handlers)
               select unit;
    }
}
