using Infrastructure.Effects.Traits;

using LanguageExt.Traits;

namespace Infrastructure.Effects;
public static class DateTimeIO<M, RT> where M : MonadIO<M>, Fallible<M> where RT : Has<M, IDateTimeIO>
{
    private static K<M, IDateTimeIO> Trait => Has<M, RT, IDateTimeIO>.ask;

    public static K<M, DateTime> Now =>

        from t in Trait
        select t.Now;


    public static K<M, DateTime> UtcNow =>
        from t in Trait
        select t.UtcNow;

    public static K<M, Unit> SleepUntil(DateTime dt) =>
        from t in Trait

        from a in dt <= t.Now
            ? M.Pure(unit)
            : M.LiftIO(IO.liftAsync<Unit>(async () =>
            {
                await Task.Delay(dt - t.Now).ConfigureAwait(false);
                return unit;
            }))

        select a;



    public static K<M, Unit> SleepFor(TimeSpan ts) =>
        from t in Trait

        from a in M.LiftIO(IO.liftAsync<Unit>(async () =>
            {
                await Task.Delay(ts).ConfigureAwait(false);
                return unit;
            }))

        select a;
}
