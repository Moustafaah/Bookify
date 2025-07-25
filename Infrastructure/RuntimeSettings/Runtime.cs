using Infrastructure.Effects.Impl;
using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.RuntimeSettings;

public record Runtime(RuntimeEnv Env) :
    Has<Eff<Runtime>, Config>,
    Has<Eff<Runtime>, IEmail>,
    Has<Eff<Runtime>, IDatabase>,
    Has<Eff<Runtime>, IDispatcher>,

    IDisposable

{


    public static Runtime Live() => New(new RuntimeEnv(new HttpClient(), Config.Default.Value));
    public static Runtime New(RuntimeEnv env) => new Runtime(env);



    static K<Eff<Runtime>, Config> Has<Eff<Runtime>, Config>.Ask =>
        liftEff<Runtime, Config>(rt => rt.Env.Config);
    static K<Eff<Runtime>, IEmail> Has<Eff<Runtime>, IEmail>.Ask =>
        liftEff<Runtime, IEmail>(rt => Email.Default);

    static K<Eff<Runtime>, IDatabase> Has<Eff<Runtime>, IDatabase>.Ask =>
        liftEff<Runtime, IDatabase>(rt => Database.Default);

    static K<Eff<Runtime>, IDispatcher> Has<Eff<Runtime>, IDispatcher>.Ask =>
        liftEff<Runtime, IDispatcher>(_ => Dispatcher.Default);
    public void Dispose()
    {
        Env.Dispose();
    }



}


