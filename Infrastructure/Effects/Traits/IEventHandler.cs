using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;

namespace Domain.Abstraction;
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    K<M, Unit> Handle<M, RT>(TEvent evt) where M : MonadIO<M>, Fallible<M> where RT : Has<M, IEmail>, Has<M, Config>;
}

