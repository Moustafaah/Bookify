using Infrastructure.Outbox;

using TableDependency.SqlClient.Base.EventArgs;
namespace Test;


internal class Program
{


    static async Task Main(string[] args)
    {

    }

    private static void OnChanged(object sender, RecordChangedEventArgs<OutboxMessage> e)
    {
        Console.WriteLine(
            $"{nameof(ProcessOutboxMessages)}: process message {e.Entity.Id}: {e.Entity.Type}");
    }


}
