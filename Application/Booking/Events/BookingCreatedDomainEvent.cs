using Domain.Abstraction;

namespace Application.Booking.Events;
public record BookingCreatedDomainEvent(Guid BookingId) : ISuccDomainEvent
{
}
