using Dapper;

using Infrastructure.Effects.Traits;

using Microsoft.Data.SqlClient;

namespace Infrastructure.Effects.Impl;

public class Database : IDatabase
{
    public static IDatabase Default => new Database();
    public async Task<A?> QuerySingle<A>(string query, object parameters, string connectionString, CancellationToken token)
    {
        await using var sqlConn = new SqlConnection(connectionString);
        await sqlConn.OpenAsync(token);
        return await sqlConn.QueryFirstOrDefaultAsync<A>(query, parameters).ConfigureAwait(false);
    }

    public async Task<IEnumerable<A>> QueryMultiple<A>(string query, object parameters, string connectionString, CancellationToken token)
    {
        await using var sqlConn = new SqlConnection(connectionString);
        await sqlConn.OpenAsync(token);
        return await sqlConn.QueryAsync<A>(query, parameters).ConfigureAwait(false);
    }

    public async Task<int> Execute(CommandDefinition command, string connectionString, CancellationToken token)
    {
        await using var sqlConn = new SqlConnection(connectionString);
        await sqlConn.OpenAsync(token);
        return await sqlConn.ExecuteAsync(command);
    }


}