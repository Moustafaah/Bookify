using Domain.Bookings.ValueObjects;

namespace Application.Booking;
public class PeriodViewModel
{
    public int Day { get; init; }
    public int Week { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
}


public static partial class ViewModel
{
    public static PeriodViewModel ToViewModel(this Period period)
    {
        return new PeriodViewModel()
        {
            Day = period.Day,
            Week = period.Week,
            Month = period.Month,
            Year = period.Year
        };
    }
}