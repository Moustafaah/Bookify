using Application.Messaging;

namespace Application.Booking.Commands;
internal sealed class CreateBookingHandler : ICommandHandler<CreateBookingRequest, Guid>
{
    //public async Task<Fin<Domain.Bookings.Booking>> Handle(CreateBooking request, CancellationToken cancellationToken)
    //{
    //    return (await (from u in UserRepo.GetUserById(request.UserId)
    //                   from a in ApartmentRepo.GetApartmentById(request.ApartmentId)
    //                   from b in Domain.Monads.Db.Db<BookifyRT>.lift(Create(a, u, request.From, request.To))
    //                   from _ in BookingRepo.IsOverlapping(a.Id, b)
    //                   from id in BookingRepo.AddBooking(b)
    //                   from __ in Domain.Monads.Db.Db<BookifyRT>.Do(() => a.ChangeLastBookedAt(DateTime.Now))
    //                   select b)
    //        .RunSaveAsync(EnvIO.New(null, cancellationToken)))
    //        .RaiseDomainEvent(b => new BookingCreatedDomainEvent(b), new CreateBookingFailEventHandler(request.UserId, request.ApartmentId));

    //}
    public Task<Fin<Guid>> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}