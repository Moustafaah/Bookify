using System.Numerics;

using LanguageExt.Traits.Domain;

namespace Domain.Users.ValueObjects;

public record Lastname : DomainType<Lastname, string>, IEqualityOperators<Lastname, string, bool>
{
    public readonly string Repr;

    private Lastname(string repr)
    {
        Repr = repr;
    }

    public static Fin<Lastname> From(string repr)
    {
        return (IsNullOrEmpty(repr).Bind(_ => IsNullOrWhiteSpace(repr)),
            MaxLength50(repr)).Apply((_, _) => new Lastname(repr)).As();
    }

    public string To()
    {
        return Repr;
    }


    public static bool operator ==(Lastname? left, string? right)
    {
        return left is { } l && right is { } r && String.Equals(l.Repr, r, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator !=(Lastname? left, string? right)
    {
        return !(left == right);
    }


    public static Lastname FromUnsafe(string repr)
    {
        return new Lastname(repr);
    }
}