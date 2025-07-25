namespace Infrastructure.Outbox;
public class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime OccuredOn { get; init; }
    public string Content { get; init; }
    public string Type { get; init; }

    public DateTime? ProcessedOn { get; init; }
    public string? Error { get; init; }

    public OutboxMessage()
    {

    }
}
