using Infrastructure.Monads.Db;

using LanguageExt.Traits;


namespace Domain.Monads.Db;

public partial class Db<RT> :
    MonadIO<Db<RT>>,
    Readable<Db<RT>, RT>,
    Choice<Db<RT>>,
    Fallible<Error, Db<RT>>
//MonadUnliftIO<Db<RT>>

{



    ////////////////////////////////////// Functor //////////////////////////////

    static K<Db<RT>, B> Functor<Db<RT>>.Map<A, B>(Func<A, B> f, K<Db<RT>, A> ma) =>
        ma.As().Map(f);


    ////////////////////////////////////// Applicative //////////////////////////////

    static K<Db<RT>, A> Applicative<Db<RT>>.Pure<A>(A value) =>
        Db<RT, A>.Pure(value);

    static K<Db<RT>, B> Applicative<Db<RT>>.Apply<A, B>(K<Db<RT>, Func<A, B>> mf, K<Db<RT>, A> ma) =>
        new Db<RT, B>(mf.As().Effect.Apply(ma.As().Effect));



    ////////////////////////////////////// Monad //////////////////////////////
    static K<Db<RT>, B> Monad<Db<RT>>.Bind<A, B>(K<Db<RT>, A> ma, Func<A, K<Db<RT>, B>> f) =>
        ma.As().Bind(f);



    ////////////////////////////////////// IO //////////////////////////////
    //static K<Db<RT>, A> MonadIO<Db<RT>>.liftIO<A>(IO<A> ma)
    //{
    //    return liftIO(ma);
    //}

    ////////////////////////////////////// Reader //////////////////////////////
    static K<Db<RT>, A> Readable<Db<RT>, RT>.Asks<A>(Func<RT, A> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.Lift(f));

    static K<Db<RT>, A> Readable<Db<RT>, RT>.Local<A>(Func<RT, RT> f, K<Db<RT>, A> ma) =>
        new Db<RT, A>(ma.As().Effect.Local(f).As());


    ////////////////////////////////////// Choice //////////////////////////////
    static K<Db<RT>, A> SemigroupK<Db<RT>>.Combine<A>(K<Db<RT>, A> lhs, K<Db<RT>, A> rhs) =>
        lhs.Choose(rhs);

    static K<Db<RT>, A> Choice<Db<RT>>.Choose<A>(K<Db<RT>, A> fa, K<Db<RT>, A> fb) =>
        new Db<RT, A>(fa.As().Effect
            .Choose(fb.As().Effect).As());

    static K<Db<RT>, A> Choice<Db<RT>>.Choose<A>(K<Db<RT>, A> fa, Func<K<Db<RT>, A>> fb)
    {
        return new Db<RT, A>(fa.As().Effect
            .Choose(fb().As().Effect).As());
    }


    ////////////////////////////////////// Failing //////////////////////////////
    static K<Db<RT>, A> Fallible<Error, Db<RT>>.Fail<A>(Error error) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.Fail(error));


    static K<Db<RT>, A> Fallible<Error, Db<RT>>.Catch<A>(K<Db<RT>, A> fa, Func<Error, bool> Predicate, Func<Error, K<Db<RT>, A>> Fail) =>
        lift(fa.As().Effect.Catch(Predicate, error => Fail(error).As().Effect).As());




    ////////////////////////////////////// lifting //////////////////////////////

    public static Db<RT, A> pure<A>(A value) =>
        Db<RT, A>.Pure(value);


    public static Db<RT, A> lift<A>(A value) =>
        Db<RT, A>.Pure(value);


    public static Db<RT, A> lift<A>(Func<RT, A> f) =>
        new(LanguageExt.Eff<RT, A>.Lift(f));


    public static Db<RT, A> lift<A>(Eff<RT, A> effect) =>
        new(effect);


    public static Db<RT, A> lift<A>(Func<RT, Fin<A>> f) =>
        new(LanguageExt.Eff<RT, A>.Lift(f));

    public static Db<RT, A> lift<A>(Fin<A> ma) =>
        new(LanguageExt.Eff<RT, A>.Lift(() => ma));




    ////////////////////////////////////// Reader //////////////////////////////
    public static Db<RT, RT> ask() =>
        Readable.ask<Db<RT>, RT>().As();

    public static Db<RT, RT> envDb => ask();

    public static Db<RT, A> asks<A>(Func<RT, A> f) =>
        Readable.asks<Db<RT>, RT, A>(f).As();

    public static Db<RT, A> local<A>(Func<RT, RT> f, Db<RT, A> dba) =>
        Readable.local(f, dba).As();



    ////////////////////////////////////// Lift IO //////////////////////////////
    public static Db<RT, A> liftIO<A>(IO<A> ma) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(ma));


    public static Db<RT, A> liftIO<A>(Func<EnvIO, Task<A>> f) =>
        new Db<RT, A>(IO.liftAsync(async envIo => await f(envIo).ConfigureAwait(false)));

    public static Db<RT, A> liftIO<A>(Func<RT, EnvIO, Task<A>> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(rt => IO.liftAsync(e => f(rt, e))));




    public static Db<RT, A> liftIO<A>(Func<RT, Task<A>> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(rt => IO.liftAsync(_ => f(rt))));



    public static Db<RT, A> liftVIO<A>(Func<RT, ValueTask<A>> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(rt => IO.liftVAsync(_ => f(rt))));


    public static Db<RT, A> liftVIO<A>(Func<RT, EnvIO, ValueTask<A>> f) =>
        new Db<RT, A>(LanguageExt.Eff<RT, A>.LiftIO(rt => IO.liftVAsync(e => f(rt, e))));

    public static Db<RT, A> fail<A>(Error error) =>
        new(LanguageExt.Eff<RT, A>.Fail(error));




    /////////////////////////  Side Effect /////////////


    //public static Db<RT, A> Do<A>(Func<A> fn) =>
    //    new(LanguageExt.Eff<RT, A>.Lift(fn));

    public static K<Db<RT>, IO<A>> ToIO<A>(K<Db<RT>, A> ma)
    {
        return ma.As().Map(a => IO.lift(() => a));
    }

    static K<Db<RT>, A> MonadIO<Db<RT>>.LiftIO<A>(IO<A> ma)
    {
        return liftIO(ma);
    }

    //static K<Db<RT>, A> MonadIO<Db<RT>>.liftIO<A>(IO<A> ma)
    //{
    //    return liftIO(ma);
    //}
}