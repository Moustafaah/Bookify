namespace Infrastructure.Effects.Traits;

public interface IDateTimeIO
{

    DateTime Now { get; }


    DateTime UtcNow { get; }


    //Unit SleepUntil(DateTime dt);


    //Unit SleepFor(TimeSpan ts);

}