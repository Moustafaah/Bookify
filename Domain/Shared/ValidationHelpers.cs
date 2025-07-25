using LanguageExt.Traits.Domain;
using LanguageExt.UnsafeValueAccess;

namespace Domain.Shared;
public static class ValidationHelpers
{
    public static bool InRangeInt(int check, int start, int end)
    {
        return start <= check && check <= end;
    }

    public static Fin<Option<A>> ValidateIfNotNull<A, B>(B? repr) where A : DomainType<A, B> where B : struct
    {
        return repr.HasValue
            ? A.From(repr.Value).Map(Some)
            : FinSucc(Option<A>.None);

    }



    public static Option<A> OptionFromUnsafe<A, B>(B? repr) where A : DomainType<A, B> where B : struct
    {
        return Optional(repr)
            .Map(A.FromUnsafe);
    }

    public static Option<A> OptionFromUnsafe<A, B>(B? repr) where A : DomainType<A, B> where B : class
    {
        return Optional(repr)
            .Map(A.FromUnsafe);
    }

    public static B? NullIfNone<A, B>(Option<A> @this) where A : DomainType<A, B> where B : struct
    {
        return @this.Match(a => a.To(), () => (B?)null);
    }

    public static A? NullIfNone<A>(Option<A> @this)
    {
        return @this.ValueUnsafe();
    }

    public static B? NullIfNoneString<A, B>(this Option<A> @this) where A : DomainType<A, B> where B : class
    {
        return @this.Match(a => a.To(), () => (B?)null);
    }




    public static Fin<Option<A>> FromNullable<A, B>(B? repr) where A : DomainType<A, B> where B : struct
    {
        return repr.HasValue
            ? A.From(repr.Value).Map(Some)
            : Option<A>.None;

    }
    public static Fin<Option<A>> FromNullable<A, B>(B? repr) where A : DomainType<A, B> where B : class
    {
        return repr is not null
            ? A.From(repr).Map(Some)
            : Option<A>.None;

    }

    public static Func<string, Fin<Unit>> MaxLength(int maxlength) =>
        repr =>
             repr.Length > maxlength
                ? FinFail<Unit>(ValidationErrors.Domain.String.MaxLength(repr, maxlength))
                : unit;


    public static Fin<Unit> MaxLength200(string repr)
    {
        return MaxLength(200)(repr);
    }
    public static Fin<Unit> MaxLength50(string repr)
    {
        return MaxLength(50)(repr);
    }
    public static Fin<Unit> MinLength10(string repr)
    {
        return MinLength(10)(repr);
    }
    public static Fin<Unit> MinLength50(string repr)
    {
        return MinLength(50)(repr);
    }
    public static Func<string, Fin<Unit>> MinLength(int minlength) =>
        repr =>
             repr.Length < minlength
                ? FinFail<Unit>(ValidationErrors.Domain.String.MinLength(repr, minlength))
                : unit;

    public static Fin<Unit> ParseEnum<T>(string repr) where T : Enum
    {
        return
            Enum.TryParse(typeof(T), repr, true, out _)
                ? unit
                : FinFail<Unit>(ValidationErrors.Domain.Enum.ParseFailure<T>(repr));
    }

    public static Fin<Unit> IsNullOrEmpty(string repr)
    {
        return string.IsNullOrEmpty(repr)
            ? FinFail<Unit>(ValidationErrors.Domain.String.IsNullOrEmpty)
            : unit;
    }
    public static Fin<Unit> IsNullOrWhiteSpace(string repr)
    {
        return string.IsNullOrWhiteSpace(repr)
            ? FinFail<Unit>(ValidationErrors.Domain.String.IsNullOrEmpty)
            : unit;
    }
}
