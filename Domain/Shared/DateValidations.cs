namespace Domain.Shared;

public delegate DateTime DateTimeUtc();
public static class DateValidations
{
    public static Fin<Unit> NotInPast(this DateTime dateTime, DateTime utcNow, string propName, string message)
    {
        return dateTime < utcNow ? FinFail<Unit>(ValidationErrors.Domain.Date.ShouldNotBeInPast(message)) : unit;
    }
    public static Fin<Unit> AtLeastOneDayDiff(this DateTime from, DateTime to, string message, string propName)
    {
        return (to - from).Days >= 1 ? unit : FinFail<Unit>(ValidationErrors.Domain.Date.AtLeastOneDayDiff(message));
    }
}