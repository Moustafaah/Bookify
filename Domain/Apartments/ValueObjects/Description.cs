using LanguageExt.Traits.Domain;

namespace Domain.Apartments.ValueObjects;

public record Description : DomainType<Description, string>
{
    public readonly string Repr;

    private Description(string repr)
    {
        Repr = repr;
    }
    public static Fin<Description> From(string repr)
    {
        return (MinLength50(repr),
                MaxLength200(repr))
            .Apply((_, _) => new Description(repr)).As();
    }

    public string To()
    {
        return Repr;
    }

    public static Description FromUnsafe(string repr)
    {
        return new Description(repr);
    }
}