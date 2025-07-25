using System.Numerics;

using LanguageExt.Traits.Domain;

namespace Domain.Users.ValueObjects;
public record Firstname : DomainType<Firstname, string>, IEqualityOperators<Firstname, string, bool>
{
    public readonly string Repr;

    private Firstname(string repr)
    {
        Repr = repr;
    }
    public static Fin<Firstname> From(string repr)
    {
        return (IsNullOrEmpty(repr).Bind(_ => IsNullOrWhiteSpace(repr)),
            MaxLength50(repr)).Apply((_, _) => new Firstname(repr)).As();
    }

    public string To()
    {
        return Repr;
    }


    public static bool operator ==(Firstname? left, string? right)
    {
        return left is { } l && right is { } r && String.Equals(l.Repr, r, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator !=(Firstname? left, string? right)
    {
        return !(left == right);
    }

    public static Firstname FromUnsafe(string repr)
    {
        return new Firstname(repr);
    }
}
