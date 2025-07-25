using Domain.Bookings.ValueObjects;

namespace Application.Booking;
public class DateRangeViewModel
{
    public int DurationInDays => (To - From).Days;
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}


public static partial class ViewModel
{
    public static DateRangeViewModel ToViewModel(this DateRange range)
    {
        return new DateRangeViewModel()
        {
            To = range.ToDate,
            From = range.FromDate
        };
    }

}
