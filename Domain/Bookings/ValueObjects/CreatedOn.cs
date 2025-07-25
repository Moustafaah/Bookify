namespace Domain.Bookings.ValueObjects;
public abstract record ActionOn
{
    public abstract string Name { get; }
    protected ActionOn(DateTime? dateTimeUtc)
    {
        On = dateTimeUtc;
    }
    public DateTime? On { get; }
}
public record CreatedOn : ActionOn
{

    public CreatedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(CreatedOn);
}

public record ConfirmedOn : ActionOn
{
    public ConfirmedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(ConfirmedOn);
}
public record CancelledOn : ActionOn
{
    public CancelledOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(CancelledOn);
}

public record DeclinedOn : ActionOn
{
    public DeclinedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(DeclinedOn);
}

public record RejectedOn : ActionOn
{
    public RejectedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(RejectedOn);
}

public record ExpiredOn : ActionOn
{
    public ExpiredOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(ExpiredOn);
}

public record CheckedOutOn : ActionOn
{
    public CheckedOutOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(CheckedOutOn);
}

public record CheckedInOn : ActionOn
{
    public CheckedInOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(CheckedInOn);
}
public record CompletedOn : ActionOn
{
    public CompletedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(CompletedOn);
}

public record NoShowOn : ActionOn
{
    public NoShowOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(NoShowOn);
}

public record RefundedOn : ActionOn
{
    public RefundedOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(RefundedOn);
}


public record RescheduleOn : ActionOn
{
    public RescheduleOn(DateTime? dateTimeUtc) : base(dateTimeUtc)
    {
    }

    public override string Name => nameof(RescheduleOn);
}


