using Domain.Abstraction;
using Domain.Monads.Db;

using Infrastructure.Outbox;

using LanguageExt.Traits;

using Newtonsoft.Json;

namespace Infrastructure.Monads.Db;

public static partial class DbExtensions
{
    public static Db<RT, A> As<RT, A>(this K<Db<RT>, A> dba)
    {
        return (Db<RT, A>)dba;
    }

    public static Db<RT, B> Map<RT, A, B>(this Func<A, B> f, K<Db<RT>, A> dba)
    {
        return dba.As().Map(f);
    }


    public static Db<RT, B> Map<RT, A, B>(this Func<A, B> f, Db<RT, A> dba)
    {
        return dba.As().Map(f);
    }


    public static async Task<Fin<A>> RunAsync<RT, A>(this Db<RT, A> dba, EnvIO envIo) where RT : IAsyncDisposable, new()
    {

        await using var env = new RT();

        return await dba
            .Effect
            .RunAsync(env, envIo);

    }


    public static async Task<Fin<A>> RunSaveAsync<RT, A>(this Db<RT, A> dba, EnvIO envIo) where RT : RuntimeSettings.HasDatabase, IAsyncDisposable, new()
    {
        await using var env = new RT();
        Fin<A> result = await dba
            .Effect
            .RunAsync(env, envIo);

        if (result.IsSucc)
        {
            try
            {
                await env.DbContext.SaveChangesAsync(envIo.Token);
            }
            catch (Exception e)
            {

                return FinFail<A>(e);
            }

        }

        return result;
    }



    public static async Task<Fin<A>> RunTransactionAsync<RT, A>(this Db<RT, A> dba, EnvIO envIo)
        where RT : RuntimeSettings.HasDatabase, IAsyncDisposable, new()
    {
        await using var env = new RT();
        await using var transaction = await env.DbContext.Database.BeginTransactionAsync(envIo.Token);

        var result = await dba
            .Effect.RunAsync(env, envIo);

        if (result.IsSucc)
        {
            await env.DbContext.SaveChangesAsync(envIo.Token);
            await transaction.CommitAsync(envIo.Token);
        }
        else
        {
            await transaction.RollbackAsync(envIo.Token);
        }

        return result;

    }

    public static K<Db<RT>, B> Apply<RT, A, B>(this Db<RT, Func<A, B>> mf, Db<RT, A> ma)
    {
        return mf.As().Bind(f => ma.Map(f));
    }

    public static K<Db<RT>, B> Apply<RT, A, B>(this Db<RT, Func<A, B>> mf, K<Db<RT>, A> ma)
    {
        return mf.Bind(f => ma.As().Map(f));
    }


    public static Db<RT, C> SelectMany<RT, A, B, C>(this Db<RT, A> dba, Func<A, Db<RT, B>> f, Func<A, B, C> selector)
    {
        return dba.Bind(a => f(a).Map(b => selector(a, b)));
    }


    public static Db<RT, C> SelectMany<RT, A, B, C>(this Db<RT, A> dba, Func<A, K<Db<RT>, B>> f, Func<A, B, C> selector)
    {
        return dba.Bind(a => f(a).As().Map(b => selector(a, b)));
    }

    public static Db<RT, C> SelectMany<RT, A, B, C>(this Db<RT, A> dba, Func<A, IO<B>> f, Func<A, B, C> selector)
    {
        return dba.As().Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, B>.LiftIO(f(a)).Map(b => selector(a, b))));
    }

    public static Db<RT, C> SelectMany<RT, A, B, C>(this Db<RT, A> dba, Func<A, Pure<B>> f, Func<A, B, C> selector) =>
        dba.Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, B>.Pure(f(a).Value).Map(b => selector(a, b))));


    public static Db<RT, C> SelectMany<RT, A, B, C>(this Db<RT, A> dba, Func<A, Ask<RT, B>> f, Func<A, B, C> selector)
    {
        return dba.Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, A>.Pure(a).SelectMany(f, selector)));
    }

    public static Db<RT, C> SelectMany<RT, A, C>(this Db<RT, A> dba, Func<A, Guard<Error, Unit>> f, Func<A, Unit, C> selector) =>
        dba.As().Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, A>
        .Pure(a)
        .SelectMany(f, selector)));



    public static Db<RT, C> SelectMany<RT, A, B, C>(this Fin<A> fa, Func<A, Db<RT, B>> f, Func<A, B, C> selector) =>
        fa.Match(a => f(a).Map(b => selector(a, b)), Db<RT>.fail<C>);

    public static Db<RT, C> SelectMany<RT, A, B, C, E>(this Validation<Error, A> fa, Func<A, Db<RT, B>> f,
        Func<A, B, C> selector) =>
        fa.Match(a => f(a).Map(b => selector(a, b)), e => Db<RT>.fail<C>(e));


    public static Db<RT, A> RaiseDomainEvent<RT, A>(this Db<RT, A> dba, Func<Guid, ISuccDomainEvent> fn) where A : Entity
    {
        return dba.Map(a =>
        {
            a.RaiseDomainEvent(fn(a.Id));
            return a;
        });
    }

    public static async Task<Fin<A>> RaiseFailDomainEvent<RT, A>(this Fin<A> ma, params IFailDomainEvent[] failEvents) where RT : RuntimeSettings.HasDatabase, IAsyncDisposable, new()
    {
        await using var rt = new RT();
        var outboxMessages = failEvents.Select(ev => new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            Error = ev.Error.Message,
            OccuredOn = DateTime.Now,
            Type = ev.GetType().Name,
            Content = JsonConvert.SerializeObject(ev,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })
        });

        await rt.DbContext.AddRangeAsync(outboxMessages);
        await rt.DbContext.SaveChangesAsync();
        return ma;

    }
}