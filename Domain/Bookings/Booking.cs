using System.ComponentModel.DataAnnotations.Schema;

using Domain.Abstraction;
using Domain.Apartments;
using Domain.Apartments.ValueObjects;
using Domain.Bookings.ValueObjects;
using Domain.Users;

//using Domain.Users.Entity;

namespace Domain.Bookings;
public record Booking : Entity
{

    public Guid ApartmentId { get; }
    public Guid UserId { get; }
    public DateRange Duration { get; }
    public Money PriceForPeriod { get; }
    public Money AmenitiesUpCharge { get; }

    public BookingStatus BookingStatus { get; }
    public Period Period { get; }

    private double? _cleaningFee;

    [NotMapped]
    public Option<Money> CleaningFee
    {
        get => Money.FromNullable(_cleaningFee);
        set => _cleaningFee = value.Match<double?>(m => m.Value, () => null);
    }
    public DateTime? CreatedOn { get; private set; }

    public DateTime? CancelledOn { get; private set; }

    public DateTime? ConfirmedOn { get; private set; }

    public DateTime? DeclinedOn { get; private set; }
    public DateTime? ExpiredOn { get; private set; }
    public DateTime? CheckedInOn { get; private set; }
    public DateTime? CheckedOutOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public DateTime? RejectedOn { get; private set; }
    public DateTime? NoShowOn { get; private set; }
    public DateTime? RefundedOn { get; private set; }
    public DateTime? RescheduleOn { get; private set; }

    public Apartment Apartment { get; }
    public User User { get; }
    private Booking() : base(Guid.NewGuid()) { }
    private Booking(
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Option<Money> cleaningFee,
        Money amenitiesUpCharge,
        BookingStatus bookingStatus,
        Period period,
        DateTime? createdOn)
        : base(Guid.NewGuid())
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        BookingStatus = bookingStatus;
        CreatedOn = createdOn;
        Period = period;
    }


    public static Fin<Booking> Create(
        Apartment apartment,
        User user,
        DateTime from,
        DateTime to)
    {
        var result = DateRange.From(from, to).Bind(dateRange =>
                    CalculateAmenitiesUpCharge(dateRange, apartment).Map(upCharge => (dateRange, upCharge)));

        return (result,
                BookingStatus.From(Status.Pending, new CreatedOn(DateTime.Now)))
            .Apply((r, bookingStatus) =>
            {
                var period = Period.From(r.dateRange.DurationLengthInDays, DateTime.Now);
                var priceForPeriod = apartment.Price * r.dateRange.DurationLengthInDays;
                return new Booking(
                    apartment.Id,
                    user.Id,
                    r.dateRange,
                    priceForPeriod,
                    apartment.CleaningFee,
                    r.upCharge,
                    bookingStatus,
                    period,
                    bookingStatus.ActionOn.On);
            }).As();
    }


    public static Fin<Money> CalculateAmenitiesUpCharge(DateRange dateRange, Apartment apartment)
    {
        return apartment.Amenities.Traverse(amenity => amenity.CalculateCost(dateRange))
            .Map(seq => seq.Fold(Money.Zero, (s, m) => s + m)).As();

    }

}


