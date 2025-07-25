using Domain.Abstraction;

using Infrastructure.Effects;
using Infrastructure.RuntimeSettings;

using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace Infrastructure.Outbox;

public sealed class ProcessOutboxMessages : BackgroundService
{
    private readonly IEnumerable<object> _handlers;

    public ProcessOutboxMessages(IEnumerable<object> handlers)
    {
        _handlers = handlers;
    }
    private SqlTableDependency<OutboxMessage> _tableDependency { get; set; }

    public JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    };



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var connectionString = Config.Default.Value.ConnectionString.IfNone(() => throw new InvalidOperationException("no connection string available"));
        _tableDependency = new SqlTableDependency<OutboxMessage>(connectionString, "outbox_messages");

        _tableDependency.OnChanged += OnChanged;
        _tableDependency.OnError += (s, e) => Console.WriteLine($"Error: {e.Error.Message}");

        Console.WriteLine("Starting SqlTableDependency...");
        _tableDependency.Start();


        stoppingToken.Register(() => { _tableDependency.Stop(); _tableDependency.Dispose(); });


        await Task.CompletedTask;

    }


    void OnChanged(object sender, RecordChangedEventArgs<OutboxMessage> e)
    {
        Console.WriteLine($"+++++++++++++++++ Starting ProcessOutboxMessages ++++++++++++++++++++++++++");
        Console.WriteLine($"there are number of: {(_handlers.Count())} handlers");
        if (e.ChangeType is ChangeType.Insert)
        {

            var x = Optional(JsonConvert.DeserializeObject<IDomainEvent>(e.Entity.Content, JsonSerializerSettings))
                .Match(evt => Dispatcher<Eff<Runtime>, Runtime>.Dispatch(evt, _handlers), () =>
                    LanguageExt.Eff<Runtime, Unit>.Lift(
                        () =>
                        {
                            Console.WriteLine(
                                $"{nameof(ProcessOutboxMessages)}: failed to process message {e.Entity.Id}: {e.Entity.Type}");
                            return unit;
                        })).Run(Runtime.Live()).Match(u => Console.WriteLine("Event Successfully processed"), er =>
                    Console.WriteLine($"An error happened while processing Event with Id {e.Entity.Id} with error: {er.Message} "));
        }
    }


}
