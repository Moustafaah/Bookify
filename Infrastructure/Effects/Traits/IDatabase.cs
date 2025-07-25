using Dapper;

namespace Infrastructure.Effects.Traits;
public interface IDatabase
{
    public Task<A?> QuerySingle<A>(string query, object parameters, string connectionString, CancellationToken token);
    public Task<IEnumerable<A>> QueryMultiple<A>(string query, object parameters, string connectionString, CancellationToken token);
    public Task<int> Execute(CommandDefinition command, string connectionString, CancellationToken token);
}