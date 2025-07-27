using Domain.Abstraction;

using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;


namespace Infrastructure.Abstractions;
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    K<M, Unit> Handle<M, RT>(TEvent evt) where M : MonadIO<M>, Fallible<M>
        where RT : Has<M, IEmail>, Has<M, IDatabase>, Has<M, Config>;
}

