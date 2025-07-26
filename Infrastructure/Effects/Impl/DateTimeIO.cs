using Infrastructure.Effects.Traits;

namespace Infrastructure.Effects.Impl;

public class DateTimeIO : IDateTimeIO
{
    private readonly DateTime _dateTime;

    public static IDateTimeIO Default(DateTime dateTime) => new DateTimeIO(dateTime);

    private DateTimeIO(DateTime dateTime)
    {
        _dateTime = dateTime;
    }
    public DateTime Now => _dateTime;
    public DateTime UtcNow => _dateTime.ToUniversalTime();

}