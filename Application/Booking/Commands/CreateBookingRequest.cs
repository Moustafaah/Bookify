using Application.Messaging;

namespace Application.Booking.Commands;
public record CreateBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateTime From,
    DateTime To) : ICommand<Guid>
{
}
