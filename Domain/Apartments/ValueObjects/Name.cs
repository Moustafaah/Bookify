using LanguageExt.Traits.Domain;

namespace Domain.Apartments.ValueObjects;

public record Name : DomainType<Name, string>
{
    public readonly string Repr;

    private Name(string repr)
    {
        Repr = repr;
    }

    public static Fin<Name> From(string repr)
    {
        return (MinLength10(repr),
                MaxLength50(repr))
            .Apply((_, _) => new Name(repr)).As();
    }

    public string To()
    {
        return Repr;
    }

    public static Name FromUnsafe(string repr)
    {
        return new Name(repr);
    }
}