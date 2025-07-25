using Domain.Abstraction;

namespace Domain.Bookings.Events;
public record BookingCreatedDomainEvent(Booking Booking) : ISuccDomainEvent
{
}
