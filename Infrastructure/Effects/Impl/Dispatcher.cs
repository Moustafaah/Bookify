using Domain.Abstraction;

using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.Effects.Impl;

public class Dispatcher : IDispatcher
{
    public static IDispatcher Default => new Dispatcher();
    public Dispatcher()
    {

    }

    public K<M, Unit> Dispatch<M>(IDomainEvent domainEvent, IEnumerable<object> handlers) where M : MonadIO<M>, Fallible<M>
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());

        var tasks = handlers.Where(h => handlerType.IsInstanceOfType(h)).Select(h =>
            (Task<Unit>)handlerType.GetMethod("Handle")!.Invoke(h, [domainEvent])!);

        return M.LiftIO(awaitAll(toSeq(tasks).Map(task => IO.liftAsync(() => task)))).Map(_ => unit);

    }
}