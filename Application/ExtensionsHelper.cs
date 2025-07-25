using Domain.Abstraction;

using LanguageExt.Common;

namespace Application;
public static class ExtensionsHelper
{

    public static Fin<A> RaiseDomainEvent<A>(this Fin<A> ma, Func<A, ISuccDomainEvent> success,
      Func<Error, IFailDomainEvent>? fail = null) where A : Entity
    {

        ma.Match(a => a.RaiseDomainEvent(success(a)), e =>
        {
            if (fail != null)

                fail(e);
        });

        return ma;
    }


    public static async Task<Fin<A>> RaiseDomainEvent<A>(this Task<Fin<A>> ma, Func<A, ISuccDomainEvent> success,
        Func<Error, IFailDomainEvent>? fail = null) where A : Entity
    {
        var fin = await ma;
        fin.Match(a => a.RaiseDomainEvent(success(a)), e =>
        {
            if (fail != null)

                fail(e);
        });

        return fin;
    }



}
