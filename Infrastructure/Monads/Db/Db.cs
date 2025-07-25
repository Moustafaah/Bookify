using Domain.Monads.Db;

using LanguageExt.Traits;

namespace Infrastructure.Monads.Db;

public record Db<RT, A>(Eff<RT, A> Effect) : K<Db<RT>, A>, Fallible<Db<RT, A>, Db<RT>, Error, A>
{
    public static Db<RT, A> Pure(A value) =>
        new(LanguageExt.Eff<RT, A>.Pure(value));

    public Db<RT, B> Map<B>(Func<A, B> f) =>
        new(Effect.Map(f));

    public Db<RT, B> Bind<B>(Func<A, Db<RT, B>> f) =>
        new(Effect.Bind(a => f(a).Effect));


    ////////////////////////////// Lifting /////////////////////////////

    public static Db<RT, A> LiftIO(IO<A> ma) =>
        new(LanguageExt.Eff<RT, A>.LiftIO(ma));

    public static Db<RT, A> LiftIO(Func<RT, IO<A>> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(f));

    //public static Db<RT, A> LiftIO(Func<RT, EnvIO, IO<A>> f) =>
    //    new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(rt => f(rt, EnvIO.New())));
    public static Db<RT, A> LiftIO(Func<RT, Task<A>> f) =>
        new(LanguageExt.Eff<RT, A>.LiftIO(f));


    public static Db<RT, A> LiftIO(Func<RT, Task<Fin<A>>> f) =>
        new(LanguageExt.Eff<RT, A>.LiftIO(f));

    public static Db<RT, A> LiftIO(Func<Task<A>> f) =>
        new(LanguageExt.Eff<RT, A>.LiftIO(f));

    public static Db<RT, A> LiftIO(Func<Task<Fin<A>>> f) =>
        new(LanguageExt.Eff<RT, A>.LiftIO(f));



    ///////////////////////////////// Linq Support ////////////////////////////////////

    public Db<RT, B> Select<B>(Func<A, B> f) => Map(f);


    public Db<RT, C> SelectMany<B, C>(Func<A, Db<RT, B>> f, Func<A, B, C> selector) =>
        Bind(a => f(a).Map(b => selector(a, b)));

    public Db<RT, C> SelectMany<B, C>(Func<A, K<Db<RT>, B>> f, Func<A, B, C> selector) =>
        Bind(a => f(a).As().Map(b => selector(a, b)));

    public Db<RT, C> SelectMany<B, C>(Func<A, IO<B>> f, Func<A, B, C> selector) =>
        Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, B>.LiftIO(f(a))
            .Map(b => selector(a, b))));

    public Db<RT, C> SelectMany<B, C>(Func<A, Pure<B>> f, Func<A, B, C> selector) =>
        Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, B>.Pure(f(a).Value).Map(b => selector(a, b))));

    public Db<RT, C> SelectMany<B, C>(Func<A, Ask<RT, B>> f, Func<A, B, C> selector) =>
        Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, A>.Pure(a).SelectMany(f, selector)));

    public Db<RT, C> SelectMany<C>(Func<A, Guard<Error, Unit>> f, Func<A, Unit, C> selector) =>
        Bind(a => new Db<RT, C>(LanguageExt.Eff<RT, A>
        .Pure(a)
        .SelectMany(f, selector)));



    /////////////////////////////  Fail And Choice ///////////////////////////////////////

    public static implicit operator Db<RT, A>(Fail<Error> fail) =>
        Fallible.fail<Error, Db<RT>, A>(fail.Value).As();

    public static implicit operator Db<RT, A>(Pure<A> ma) =>
        Pure(ma.Value);

    public static Db<RT, A> Fail(Fail<Error> fail) => fail;

    public static Db<RT, A> Fail(Error e) => new Fail<Error>(e);

    public Db<RT, A> Catch(Func<Error, bool> predicate, Func<Error, Error> map) =>
        new(Effect.Catch(predicate, map).As());

    public static Db<RT, A> operator |(Db<RT, A> lhs, Db<RT, A> rhs) =>
        new(lhs.Effect.Choose(rhs.Effect).As());

    static Db<RT, A> Fallible<Db<RT, A>, Db<RT>, Error, A>.operator |(K<Db<RT>, A> lhs, Db<RT, A> rhs) =>
        lhs.As() | rhs;

    static Db<RT, A> Fallible<Db<RT, A>, Db<RT>, Error, A>.operator |(Db<RT, A> lhs, K<Db<RT>, A> rhs) =>
        lhs | rhs.As();

    public static Db<RT, A> operator |(Db<RT, A> lhs, Pure<A> rhs) =>
        lhs | Pure(rhs.Value);

    public static Db<RT, A> operator |(Db<RT, A> lhs, Fail<Error> rhs) =>
        lhs | Fail(rhs);

    public static Db<RT, A> operator |(Db<RT, A> lhs, CatchM<Error, Db<RT>, A> rhs) =>
        lhs.Catch(rhs.Match, error => rhs.Action(error)).As();




}