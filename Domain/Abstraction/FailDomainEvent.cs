using LanguageExt.Common;

namespace Domain.Abstraction;
public interface IFailDomainEvent : IDomainEvent
{
    public abstract Error Error { get; init; }
}