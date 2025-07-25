namespace Application.Booking;
public class BookingViewModel
{
    public Guid Id { get; init; }
    public Guid ApartmentId { get; init; }
    public Guid UserId { get; init; }
    public DateRangeViewModel Duration { get; init; }
    public PeriodViewModel Period { get; init; }
    public double PriceForPeriod { get; init; }
    public double? CleaningFee { get; init; }
    public double AmenitiesUpCharge { get; init; }

    public string Status { get; init; }

    public DateTime? CreatedOn { get; init; }

    public DateTime? CancelledOn { get; init; }

    public DateTime? ConfirmedOn { get; init; }

    public DateTime? RejectedOn { get; init; }

}


public static partial class ViewModel
{
    public static BookingViewModel ToViewModel(this Domain.Bookings.Booking booking)
    {
        return new BookingViewModel()
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ApartmentId = booking.ApartmentId,
            Duration = booking.Duration.ToViewModel(),
            Period = booking.Period.ToViewModel(),
            PriceForPeriod = booking.PriceForPeriod.Value,
            Status = booking.BookingStatus.Status.Name,
            AmenitiesUpCharge = booking.AmenitiesUpCharge.Value,
            CleaningFee = booking.CleaningFee.Match<double?>(money => money.Value, () => null),
            ConfirmedOn = booking.ConfirmedOn,
            CancelledOn = booking.CancelledOn,
            CreatedOn = booking.CreatedOn,
            RejectedOn = booking.RejectedOn,
        };
    }
}