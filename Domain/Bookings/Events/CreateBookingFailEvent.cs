using Domain.Abstraction;

using LanguageExt.Common;

namespace Domain.Bookings.Events;

public record CreateBookingFailEvent(Guid UserId, Guid ApartmentId, Error Error) : IFailDomainEvent
{

    public Error Error { get; init; } = Error;
}