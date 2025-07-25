using Domain.Apartments;
using Domain.Monads.Db;

using Infrastructure.DbRuntime;
using Infrastructure.Monads.Db;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;
public static class ApartmentRepo
{
    public static Db<BookifyRT, Guid> AddApartment(Apartment apartment) =>
        from e in Db<BookifyRT>.lift(rt => rt.DbContext.Apartments.Add(apartment))
        select e.Entity.Id;


    public static Db<BookifyRT, Apartment> GetApartmentById(Guid id) =>
        from a in Db<BookifyRT>.liftVIO(async (rt, e) => await rt.DbContext.Apartments.FindAsync([id], e.Token))
        from _ in when(a is null, Db<BookifyRT>.fail<Unit>(Error.New($"Apartment with id: {id} was not found.")))
        select a;


    public static Db<BookifyRT, Unit> DeleteApartment(Apartment apartment) =>
        from _ in Db<BookifyRT>.lift(rt => rt.DbContext.Apartments.Remove(apartment))
        select unit;


    public static Db<BookifyRT, Unit> FailIfHasBookings(Guid apartmentId) =>
        from b in Db<BookifyRT>.liftVIO(async (rt, e) =>
            await rt.DbContext.Apartments.Where(apartment => apartment.Id == apartmentId).AnyAsync(e.Token))
        from _ in when(b,
            Db<BookifyRT>.fail<Unit>(Error.New(
                $"Apartment with Id: {apartmentId} has bookings and can not be deleted, please cancel those bookings before preceding")))
        select unit;

}


//from a in GetApartmentById(id)
//    let bookings = a.Bookings
//    from __ in when(bookings.Any(), Db<BookifyRT>.fail<Unit>(
//        Error.New($"Apartment with id '{id} has  {bookings.Length} {(bookings.Length == 1 ? "Booking" : "Bookings")}', so it cannot be deleted")))