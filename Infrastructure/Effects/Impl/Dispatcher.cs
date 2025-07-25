using Domain.Abstraction;

using Infrastructure.Effects.Traits;

namespace Infrastructure.Effects.Impl;

public class Dispatcher : IDispatcher
{
    public static IDispatcher Default => new Dispatcher();
    public Dispatcher()
    {

    }
    //public Dispatcher(IEnumerable<object> handlers)
    //{
    //    _handlers = handlers;

    //}
    //private readonly IEnumerable<object> _handlers;

    public Task Dispatch(IDomainEvent domainEvent, IEnumerable<object> handlers)
    {

        var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());

        var tasks = handlers.Where(h => handlerType.IsInstanceOfType(h)).Select(h =>
            (Task)handlerType.GetMethod("Handle")!.Invoke(h,
                [domainEvent])!);

        return Task.WhenAll(tasks);



    }
}