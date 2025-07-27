using System.Reflection;

using Domain.Abstraction;

using Infrastructure.Abstractions;
using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.Effects.Impl;

public class Dispatcher : IDispatcher
{
    public static IDispatcher Default => new Dispatcher();

    public K<M, Unit> Dispatch<M, RT>(IDomainEvent domainEvent, IEnumerable<object> handlers) where M : MonadIO<M>, Fallible<M>
    {

        var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());


        var hs = handlers.Where(h => handlerType.IsInstanceOfType(h)).Select(o =>
        {
            MethodInfo? handle = handlerType.GetMethod("Handle");

            return (K<M, Unit>)handle!.MakeGenericMethod(typeof(M), typeof(RT)).Invoke(o, [domainEvent])!;
        });

        return toSeq(hs)
            .Traverse(m => m).Map(_ => unit);





    }
}