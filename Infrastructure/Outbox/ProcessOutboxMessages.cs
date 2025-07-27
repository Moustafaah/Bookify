using Domain.Abstraction;

using Infrastructure.Effects;
using Infrastructure.RuntimeSettings;

using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        TypeNameHandling = TypeNameHandling.All,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy(),
            IgnoreSerializableAttribute = true
        }
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


    private void OnChanged(object sender, RecordChangedEventArgs<OutboxMessage> e)
    {
        if (e.ChangeType is ChangeType.Insert)
            (from ev in Optional(JsonConvert.DeserializeObject<IDomainEvent>(e.Entity.Content, JsonSerializerSettings))
                    .ToFin(Error.New($"Could not parse content of the domain event with id: {e.Entity.Id}"))
             from _ in Dispatch(ev, _handlers)
             select unit).IfFail(er => Console.WriteLine(
                    $"An error happened while processing Event with Id {e.Entity.Id} with error: {er}"));
    }

    static Fin<Unit> Dispatch(IDomainEvent domainEvent, IEnumerable<object> handlers)
    {
        using var r = Runtime.New;
        using var r2 = Runtime.New;
        return Dispatcher<Eff<Runtime>, Runtime>.Dispatch(domainEvent, handlers).Run(r);
    }
}
