using Dapper;

using Infrastructure.Effects.Traits;
using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;

namespace Infrastructure.Effects;

public static class Database<M, RT>
    where RT : Has<M, IDatabase>, Has<M, Config>
    where M : MonadIO<M>, Fallible<M>
{
    private static K<M, IDatabase> Trait => Has<M, RT, IDatabase>.ask;

    public static K<M, A> QuerySingle<A>(string query, object parameters)
    {
        return from conn in Config<M, RT>.ConnectionString
               from db in Trait
               from result in IO.liftAsync(env => db.QuerySingle<A>(query, parameters, conn, env.Token))
               select result;
    }

    public static K<M, IEnumerable<A>> QueryMultiple<A>(string query, object parameters)
    {
        return from conn in Config<M, RT>.ConnectionString
               from db in Trait
               from result in IO.liftAsync(env => db.QueryMultiple<A>(query, parameters, conn, env.Token))
               select result;
    }

    public static K<M, int> Execute(CommandDefinition command, object parameters)
    {
        return from conn in Config<M, RT>.ConnectionString
               from db in Trait
               from result in IO.liftAsync(env => db.Execute(command, conn, env.Token))
               select result;
    }

}
