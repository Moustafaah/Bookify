using Domain.Bookings;
using Domain.Bookings.ValueObjects;
using Domain.Monads.Db;

using Infrastructure.DbRuntime;
using Infrastructure.Monads.Db;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;
public static class BookingRepo
{
    private static IReadOnlyList<Status> _status =>
        [Status.Confirmed, Status.Pending, Status.CheckedIn];

    // new Readonly[Status.Confirmed, Status.Pending, Status.CheckedIn];
    public static Db<BookifyRT, Guid> AddBooking(Booking booking)
    {
        return from e in Db<BookifyRT>.lift(rt => rt.DbContext.Bookings.Add(booking))
               select e.Entity.Id;
    }
    public static Db<BookifyRT, Booking> GetBookingById(Guid id) =>
        from a in Db<BookifyRT>.liftVIO(async (rt, e) => await rt.DbContext.Bookings.FindAsync([id], e.Token))
        from _ in when(a is null, Db<BookifyRT>.fail<Unit>(Error.New($"Booking with id: {id} was not found.")))
        select a;

    public static Db<BookifyRT, Unit> IsOverlapping(Guid apartmentId, Booking booking)
    {
        return from b in Db<BookifyRT>.liftVIO(async (rt, e) =>
                 await rt.DbContext.Bookings.Where(b =>
                  b.ApartmentId == apartmentId
                  && b.Duration.FromDate >= booking.Duration.FromDate
                  && b.Duration.ToDate <= booking.Duration.ToDate
                  && _status.Contains(b.BookingStatus.Status)
                  ).AnyAsync(e.Token))

               from _ in when(b,
                   Db<BookifyRT>.fail<Unit>(Error.New(
                       $"Booking with range from : '{booking.Duration.FromDate.ToShortDateString()}' to '{booking.Duration.ToDate.ToShortDateString()}' is overlapping")))

               select unit;
    }
    public static Db<BookifyRT, Unit> DeleteBooking(Booking booking) =>

        from _ in Db<BookifyRT>.lift(rt => rt.DbContext.Bookings.Remove(booking))
        select unit;

}
