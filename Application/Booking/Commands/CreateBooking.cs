using Application.Messaging;

namespace Application.Booking.Commands;
public record CreateBooking(
    Guid ApartmentId,
    Guid UserId,
    DateTime From,
    DateTime To) : ICommand<Domain.Bookings.Booking>
{
}
