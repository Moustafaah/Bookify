namespace Domain.Bookings.ValueObjects;

public record Status
{
    private readonly IEnumerable<Status> _allowedStatusTransition;
    private Status() { }
    public byte Code { get; }
    public string Name { get; }

    private static readonly List<Status> _all = new();
    public static IReadOnlyList<Status> All = _all;

    private Status(byte code, string name, IEnumerable<Status> allowedStatusTransition)
    {
        _allowedStatusTransition = allowedStatusTransition;
        Code = code;
        Name = name;
        _all.Add(this);
    }
    public static Status Pending => new Status(1, nameof(Pending), [Confirmed, Cancelled, Declined, Expired]);
    public static Status Confirmed => new Status(2, nameof(Confirmed), [Cancelled, Declined, CheckedIn, NoShow, Rescheduled]);
    public static Status Cancelled => new Status(3, nameof(Cancelled), [Refunded, Rescheduled]);
    public static Status Declined => new Status(4, nameof(Declined), [Refunded, Rescheduled]);
    public static Status Expired => new Status(5, nameof(Expired), []);
    public static Status CheckedIn => new Status(6, nameof(CheckedIn), [CheckedOut, Completed]);
    public static Status CheckedOut => new Status(7, nameof(CheckedOut), [Completed]);
    public static Status Completed => new Status(8, nameof(Completed), []);
    public static Status NoShow => new Status(9, nameof(NoShow), [Rescheduled]);
    public static Status Refunded => new Status(10, nameof(Refunded), []);
    public static Status Rescheduled => new Status(11, nameof(Rescheduled), []);


    public static Fin<Status> FromCode(int code)
    {
        return Optional(_all.FirstOrDefault(status => status.Code == code))
            .ToFin(ValidationErrors.Domain.Bookings.Status.Invalid($"Invalid booking status id, '{code}'"));
    }

    public static Fin<Status> FromValue(string name)
    {
        return Optional(_all.FirstOrDefault(status => status.Name == name))
            .ToFin(ValidationErrors.Domain.Bookings.Status.Invalid($"Invalid booking status, '{name}'"));
    }

    public static bool IsAllowedTransition(Status from, Status to) =>
        from._allowedStatusTransition.FirstOrDefault(status => status.Code == to.Code) is not null;
}