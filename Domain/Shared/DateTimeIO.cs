namespace Domain.Shared;

public class DateTimeIO : IDateTimeIO
{

    public static DateTimeIO Default => new DateTimeIO();

    //private static Lazy<DateTimeIO> _dateTimeIo => new Lazy<DateTimeIO>(() => new DateTimeIO());


    public IO<DateTime> Now => IO.lift(() => DateTime.Now);
    public IO<DateTime> UtcNow => IO.lift(() => DateTime.UtcNow);
    public IO<DateTime> Today => IO.lift(() => DateTime.Today);
    public IO<Unit> SleepUntil(DateTime dt)
    {
        return from now in Now
               from res in dt <= now ? unitIO : liftIO(async (e) => await Task.Delay(dt - now, e.Token).ConfigureAwait(false))
               select res;
    }

    public IO<Unit> SleepFor(TimeSpan ts)
    {
        return liftIO(async e => await Task.Delay(ts, e.Token).ConfigureAwait(false));
    }
}