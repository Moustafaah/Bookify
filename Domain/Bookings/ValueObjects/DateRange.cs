using System.Globalization;
using System.Numerics;

using LanguageExt.Traits.Domain;

namespace Domain.Bookings.ValueObjects;


// define date range for booking => validation - not in past start and end - start is before end at least one day - 
// checking for overlapping date ranges 
// DateRange should be in Ticks representing Date only



public record DateRange : ITimeSpan<DateRange, DaysTicks, DateRange>
{
    public int DurationLengthInDays => (ToDate - FromDate).Days;
    public Ticks FromTicks { get; }
    public Ticks ToTicks { get; }

    public DateTime FromDate { get; }
    public DateTime ToDate { get; }

    private DateRange(Ticks fromTicks, Ticks toTicks)
    {
        FromTicks = fromTicks;
        ToTicks = toTicks;
        FromDate = fromTicks.ToDateTime();
        ToDate = toTicks.ToDateTime();
    }

    private DateRange(DateTime fromDate, DateTime toDate)
    {
        FromDate = fromDate;
        ToDate = toDate;
        FromTicks = Ticks.From(fromDate);
        ToTicks = Ticks.From(toDate);
    }

    public static Fin<DateRange> From(DateTime from, DateTime to)
    {
        return (NotInPast(from, to), AtLeastOneDayDiff(from, to))
            .Apply((_, _) => new DateRange(from, to))
            .As();
    }

    public (Ticks From, Ticks To) ToTicksTuple()
    {
        return (FromTicks, ToTicks);
    }

    public (DateTime From, DateTime To) ToDateTuple()
    {
        return (FromDate, ToDate);
    }
    public Period ToPeriod(DateTime utc)
    {
        return Period.From((FromTicks.To() - ToTicks.To()) / Ticks.OneDay, utc);
    }

    private static Fin<Unit> NotInPast(DateTime start, DateTime end)
    {
        return
        (
            start.NotInPast(DateTime.UtcNow, nameof(DateRange),
                $"Duration should not start in past, got '{start.ToShortDateString()}',"),
            end.NotInPast(DateTime.UtcNow, nameof(DateRange),
                $"Duration should not End in past, got '{end.ToShortDateString()}'")
        ).Apply(((_, _) => unit)).As();
    }

    private static Fin<Unit> AtLeastOneDayDiff(DateTime start, DateTime end) =>
        start.AtLeastOneDayDiff(end, "Should have at least one day range", nameof(DateRange));

    //public Fin<Unit> IsOverLapping(DateRange check)
    //{
    //    return (FromTicks.To(), ToTicks.To()) switch
    //    {
    //        var (from, to) when check.FromTicks.To() >= from && check.ToTicks.To() <= to =>
    //            FinFail<Unit>(Errors.Domain.Date.IsOverlappingStart(nameof(DateRange), this, check)),
    //        var (from, to) when check.ToTicks.To() >= from && check.ToTicks.To() <= to =>
    //            FinFail<Unit>(Errors.Domain.Date.IsOverlappingEnd(nameof(DateRange), this, check)),
    //        _ => unit
    //    };

    //}

    public bool IsOverLapping(DateRange check)
    {
        return (FromTicks.To(), ToTicks.To()) switch
        {
            var (from, to) when check.FromTicks.To() >= from && check.ToTicks.To() <= to => true,
            var (from, to) when check.ToTicks.To() >= from && check.ToTicks.To() <= to => true,
            _ => false
        };

    }


    public static DateRange operator +(DateRange left, DaysTicks right)
    {
        return new DateRange(left.FromTicks, Ticks.From(left.ToTicks.To() + right.To()));
    }

    public static DateRange operator -(DateRange left, DaysTicks right)
    {
        return new DateRange(left.FromTicks, Ticks.From(left.ToTicks.To() - right.To()));
    }

    public int CompareTo(DateRange? other)  // Here only length of a range is compared in terms of days
    {
        return (this, other) switch
        {
            (null, null) => 0,
            ({ }, null) => 1,
            (null, { }) => -1,
            ({ } a, { } b) =>
                (a.FromTicks.To() - a.ToTicks.To())
                .CompareTo(b.FromTicks.To() - b.FromTicks.To()),
        };
    }

    public static bool operator ==(DateRange? left, DaysTicks right)
    {
        return left is { } l && (l.FromTicks.To() - l.ToTicks.To() == right.To());
    }

    public static bool operator !=(DateRange? left, DaysTicks right)
    {
        return !(left == right);
    }

    public static bool operator >(DateRange left, DaysTicks right)
    {
        return ((left.FromTicks.To() - left.ToTicks.To()) > right.To());
    }

    public static bool operator >=(DateRange left, DaysTicks right)
    {
        return ((left.FromTicks.To() - left.ToTicks.To()) >= right.To());
    }

    public static bool operator <(DateRange left, DaysTicks right)
    {
        return ((left.FromTicks.To() - left.ToTicks.To()) < right.To());
    }

    public static bool operator <=(DateRange left, DaysTicks right)
    {
        return ((left.FromTicks.To() - left.ToTicks.To()) <= right.To());
    }
}




public readonly record struct DaysTicks
{
    private readonly int _repr;

    private DaysTicks(int repr)
    {
        _repr = repr * Ticks.OneDay;
    }

    public int To()
    {
        return _repr;
    }


    public Ticks ToTicks()
    {
        return Ticks.From(_repr);
    }


}


public record Ticks :
ITimeSpan<Ticks, Ticks, Ticks>
{
    public static int OneDay => 864;
    public static int OneWeek => OneDay * 7;
    private readonly int _repr;

    private static int Zeros => 1_000_000_000;

    private Ticks(int repr) => _repr = repr;

    public static Ticks From(DateTime dateTime) =>
        new((int)(dateTime.Ticks / Zeros));

    public static Ticks From(int repr) =>
        new(repr);

    public int To() => _repr;


    public DateTime ToDateTime() =>
        new(_repr * Zeros);

    public static Ticks operator +(Ticks left, Ticks right) =>
        new(left.To() + right.To());

    public static Ticks operator -(Ticks left, Ticks right) =>
        new(left.To() - right.To());

    public int CompareTo(Ticks? other) =>
        (this, other) switch
        {
            (_, null) => 1,
            var (a, b) => a.To().CompareTo(b.To())
        };

    public static bool operator >(Ticks left, Ticks right) =>
        left.To() > right.To();

    public static bool operator >=(Ticks left, Ticks right) =>
        left.To() >= right.To();

    public static bool operator <(Ticks left, Ticks right) =>
        left.To() < right.To();

    public static bool operator <=(Ticks left, Ticks right) =>
        left.To() <= right.To();


}


public record WeekSpan(int Week)
// from Days
{
    public static WeekSpan From(int repr)
    {


        return new WeekSpan(repr);
    }

    public int To()
    {
        return Week;
    }


}



public record MonthSpan(int Month)
{

    public static MonthSpan From(int repr)
    {
        return new MonthSpan(repr);
    }



    public int To()
    {
        return Month;

    }


}
public readonly record struct YearSpan : Amount<YearSpan, int>
{
    private readonly int _repr;

    public YearSpan(int repr)
    {
        _repr = repr;
    }

    public static YearSpan From(int repr)
    {
        return new YearSpan(repr);
    }

    public int ToDays(DateTimeUtc dateTimeUtc)
    {
        var days = 0;
        var currentYear = dateTimeUtc().Year;
        for (int i = 0; i < To(); i++)
        {
            bool isLeapYear = DateTime.IsLeapYear(currentYear++);

            if (isLeapYear)
            {
                days += 265;
            }
            else
            {
                days += 366;
            }
        }
        return days;
    }

    public int To()
    {
        return _repr;
    }
    public static YearSpan operator -(YearSpan value)
    {
        return new YearSpan(-value._repr);
    }

    public static YearSpan operator +(YearSpan left, YearSpan right)
    {
        return From(left._repr + right._repr);
    }

    public static YearSpan operator -(YearSpan left, YearSpan right)
    {
        return From(left._repr - right._repr);
    }

    public static YearSpan operator *(YearSpan left, int right)
    {
        return From(left._repr * right);
    }

    public static YearSpan operator /(YearSpan left, int right)
    {
        return From(left._repr / right);
    }

    public int CompareTo(YearSpan other)
    {
        return To().CompareTo(other.To());
    }

    public static bool operator >(YearSpan left, YearSpan right)
    {
        return left.To() > right.To();
    }

    public static bool operator >=(YearSpan left, YearSpan right)
    {
        return left.To() >= right.To();
    }

    public static bool operator <(YearSpan left, YearSpan right)
    {
        return left.To() < right.To();
    }

    public static bool operator <=(YearSpan left, YearSpan right)
    {
        return left.To() <= right.To(); ;
    }
}

public record Period :
    ITimeSpan<Period, int, Period>,// Dealing with days
    ITimeSpan<Period, Period, Period>

{

    private Period() { }

    private readonly int _repr;

    private Period(int day, int week, int month, int year, int repr)
    {
        _repr = repr;
        Day = day;
        Week = week;
        Month = month;
        Year = year;

    }

    public int Day { get; }
    public int Week { get; }
    public int Month { get; }
    public int Year { get; }

    public static Period From(int repr, DateTime dateTimeUtc)
    {

        DateTime utc = dateTimeUtc;
        var (years, daysLeftOfAYear) = GetYears(repr, utc);
        var (months, daysLeftOfAMonth) = GetMonths(daysLeftOfAYear, utc);
        var (weeks, days) = GetWeeks(daysLeftOfAMonth);

        return new Period(days, weeks, months, years, repr);
    }

    public int To()
    {
        return _repr;
    }


    public static (int Weeks, int daysLeft) GetWeeks(int days)
    {
        return (days / 7, days % 7);
    }
    private static (int Months, int DaysLeft) GetMonths(int days, DateTime dateTime)
    {
        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        var year = calendar.GetYear(dateTime);
        var month = calendar.GetMonth(dateTime);
        var daysInCurrentMonth = calendar.GetDaysInMonth(year, month);
        var daysInNextMonth = calendar.GetDaysInMonth(year, ++month);
        var daysLeft = days;


        var months = 0;
        while ((daysLeft -= daysInCurrentMonth) >= daysInNextMonth)
        {
            months++;
        }
        return (months, daysLeft);
    }

    public static (int Years, int DaysLeft) GetYears(int days, DateTime dateTime)
    {
        var currentYear = dateTime.Year;

        var y = 0;
        var daysLeft = days;
        while (true)
        {

            var daysInYear = DateTime.IsLeapYear(currentYear) ? 365 : 366;

            if (daysLeft < daysInYear) break;
            daysLeft -= daysInYear;
            y++;
            currentYear++;

        }

        return (y, daysLeft);
    }


    public static Period operator +(Period left, int right)
    {
        return From(left._repr + right, DateTime.Now);
    }

    public static Period operator -(Period left, int right)
    {
        return From(left._repr - right, DateTime.Now);
    }

    public int CompareTo(Period? other)
    {
        return (To(), other) switch
        {
            (_, null) => 1,
            var (a, b) => a.CompareTo(b.To())
        };
    }

    public static bool operator ==(Period? left, int right)
    {
        return (left, right) switch
        {
            (null, var r) => false,
            var (a, b) => a.To().Equals(b)
        };
    }

    public static bool operator !=(Period? left, int right)
    {
        return !(left == right);
    }

    public static bool operator >(Period left, int right)
    {
        return left.To() > right;
    }

    public static bool operator >=(Period left, int right)
    {
        return left.To() >= right;
    }

    public static bool operator <(Period left, int right)
    {
        return left.To() < right;
    }

    public static bool operator <=(Period left, int right)
    {
        return left.To() <= right;
    }

    public static Period operator +(Period left, Period right)
    {
        return From(left.To() + right.To(), DateTime.Now);
    }

    public static Period operator -(Period left, Period right)
    {
        return From(left.To() - right.To(), DateTime.Now);
    }

    public static bool operator >(Period left, Period right)
    {
        return left.To() > right.To();
    }

    public static bool operator >=(Period left, Period right)
    {
        return left.To() >= right.To();
    }

    public static bool operator <(Period left, Period right)
    {
        return left.To() < right.To();
    }

    public static bool operator <=(Period left, Period right)
    {
        return left.To() <= right.To();
    }

    public static Period operator -(Period value)
    {
        return new Period(value.Day, value.Week, value.Month, -value.Year, -value.To());
    }

    //public static DateBase AdditiveIdentity => From(0, () => DateTime.Now);
    //public static DateBase operator +(DateBase left, YearSpan right)
    //{

    //    return From(left._repr + right.To(), () => DateTime.Now);
    //}

    //static YearSpan ISubtractionOperators<DateBase, DateBase, YearSpan>.operator -(DateBase left, DateBase right)
    //{
    //    throw new NotImplementedException();
    //}
}


public interface ITimeSpan<SELF, B, C> :
    IAdditionOperators<SELF, B, C>,
    ISubtractionOperators<SELF, B, C>,
    IComparable<SELF>,
    IComparisonOperators<SELF, B, bool> where SELF : ITimeSpan<SELF, B, C>;

