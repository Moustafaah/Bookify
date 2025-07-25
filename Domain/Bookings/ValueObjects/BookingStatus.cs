using LanguageExt.Traits.Domain;

namespace Domain.Bookings.ValueObjects;

public record BookingStatus : DomainType<BookingStatus, (Status Status, ActionOn On)>
{
    private BookingStatus() { }
    private BookingStatus(Status status, ActionOn actionOn)
    {
        Status = status;
        ActionOn = actionOn;
    }

    public Status Status { get; }
    public ActionOn ActionOn { get; }
    public static Fin<BookingStatus> From((Status Status, ActionOn On) repr)
    {
        return (repr.Status, repr.On) switch
        {
            var (s, o) when s == Status.Pending && o is CreatedOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Declined && o is DeclinedOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Confirmed && o is ConfirmedOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Cancelled && o is CancelledOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Expired && o is ExpiredOn => new BookingStatus(s, o),
            var (s, o) when s == Status.CheckedIn && o is CheckedInOn => new BookingStatus(s, o),
            var (s, o) when s == Status.CheckedOut && o is CheckedOutOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Completed && o is CompletedOn => new BookingStatus(s, o),
            var (s, o) when s == Status.NoShow && o is NoShowOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Refunded && o is RefundedOn => new BookingStatus(s, o),
            var (s, o) when s == Status.Rescheduled && o is RescheduleOn => new BookingStatus(s, o),
            _ => FinFail<BookingStatus>(ValidationErrors.Domain.Bookings.Status.InvalidVariant($"Invalid Status {repr.Status.Name},with {repr.On.Name}"))
        };
    }

    public static Fin<BookingStatus> From(Status status, ActionOn on)
    {
        return From((status, on));
    }

    public (Status Status, ActionOn On) To()
    {
        return (Status, ActionOn);
    }

    public Fin<BookingStatus> Update(BookingStatus to)
    {
        return Status.IsAllowedTransition(Status, to.Status)
            ? new BookingStatus(to.Status, to.ActionOn)
            : FinFail<BookingStatus>(ValidationErrors.Domain.Bookings.Status.InvalidStatusChange(
                $"Change o  f booking from {Status.Name} to {to.Status.Name} is not allowed"));
    }
}