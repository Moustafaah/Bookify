﻿namespace Domain.Shared;

public interface IDateTimeIO
{
    /// <summary>
    /// Current local date time
    /// </summary>
    IO<DateTime> Now { get; }

    /// <summary>
    /// Current universal date time
    /// </summary>
    IO<DateTime> UtcNow { get; }

    /// <summary>
    /// Today's date 
    /// </summary>
    IO<DateTime> Today { get; }

    /// <summary>
    /// Pause a task until a specified time
    /// </summary>
    IO<Unit> SleepUntil(DateTime dt);

    /// <summary>
    /// Pause a task until for a specified length of time
    /// </summary>
    IO<Unit> SleepFor(TimeSpan ts);

}