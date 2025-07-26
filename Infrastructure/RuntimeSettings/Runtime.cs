using Infrastructure.Effects.Impl;
using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.RuntimeSettings;

public record Runtime(RuntimeEnv Env) :
    Has<Eff<Runtime>, Config>,
    Has<Eff<Runtime>, IEmail>,
    Has<Eff<Runtime>, IDatabase>,
    Has<Eff<Runtime>, IDispatcher>,
    Has<IO, IDateTimeIO>,
    Has<Eff<Runtime>, IDateTimeIO>,
    //Has<Eff<Runtime>, PathIO>,
    //Has<IO, PathIO>,

    IDisposable

{


    static RuntimeEnv Common(DateTime dateTime, Config config) => new RuntimeEnv(dateTime, config);
    public static Runtime Live() => new(Common(DateTime.Now, Config.Default.Value));
    public static Runtime Test(DateTime dateTime) => new(Common(dateTime, Config.Test.Value));
    public static Runtime New => Live();



    static K<Eff<Runtime>, Config> Has<Eff<Runtime>, Config>.Ask =>
        liftEff<Runtime, Config>(rt => rt.Env.Config);
    static K<Eff<Runtime>, IEmail> Has<Eff<Runtime>, IEmail>.Ask =>
        liftEff<Runtime, IEmail>(rt => Email.Default);

    static K<Eff<Runtime>, IDatabase> Has<Eff<Runtime>, IDatabase>.Ask =>
        liftEff<Runtime, IDatabase>(rt => Database.Default);

    static K<Eff<Runtime>, IDispatcher> Has<Eff<Runtime>, IDispatcher>.Ask =>
        liftEff<Runtime, IDispatcher>(_ => Dispatcher.Default);

    static K<IO, IDateTimeIO> Has<IO, IDateTimeIO>.Ask =>
        IO.lift<IDateTimeIO>(_ => DateTimeIO.Default(New.Env.DateTime));
    static K<Eff<Runtime>, IDateTimeIO> Has<Eff<Runtime>, IDateTimeIO>.Ask =>
        liftEff<Runtime, IDateTimeIO>(_ => DateTimeIO.Default(New.Env.DateTime));

    //static K<Eff<Runtime>, PathIO> Has<Eff<Runtime>, PathIO>.Ask =>
    //    liftEff<Runtime, PathIO>(_ => PathIO.Default);

    //static K<IO, PathIO> Has<IO, PathIO>.Ask =>
    //    IO.lift(() => PathIO.Default);

    public void Dispose()
    {

    }



}



