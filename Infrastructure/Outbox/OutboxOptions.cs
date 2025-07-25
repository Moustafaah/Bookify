namespace Infrastructure.Outbox;
public class OutboxOptions
{

    public int IntervalInSec { get; set; }
    public int BatchSize { get; set; }
}
