using Infrastructure.RuntimeSettings;

using LanguageExt.Traits;
using LanguageExt.UnsafeValueAccess;

namespace Infrastructure.Effects;
public class Config<M, RT>
    where RT : Has<M, Config>
    where M : Fallible<M>, Monad<M>

{
    static K<M, Config> Trait => Has<M, RT, Config>.ask;

    public static K<M, string> ConnectionString =>
        from o in Trait.Map(t => t.ConnectionString)
        from a in when(o.IsNone, error<M>(Error.New("InvalidOperation: ConnectionString is not available")))
        select o.ValueUnsafe();
    public static K<M, string> SendGridKey =>
        from o in Trait.Map(t => t.SendGridApiKey)
        from a in when(o.IsNone, error<M>(Error.New("InvalidOperation: SendGridKey is not available")))
        select o.ValueUnsafe();
}
