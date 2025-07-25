using LanguageExt.Traits.Domain;

namespace Domain.Amenities.ValueObjects;

public record Description
    : DomainType<Description, string>
{
    public string Repr { get; }

    private Description(string repr)
    {
        Repr = repr;
    }

    public static Fin<Description> From(string repr)
    {
        var propName = "Description";
        return (MinLength50(repr), MaxLength200(repr)).Apply((_, _) => new Description(repr)).As();
    }

    public static Fin<Option<Description>> FromNullable(string? repr)
    {
        var propName = "Description";
        return repr switch
        {
            null => FinSucc(Option<Description>.None),
            _ => (MinLength50(repr), MaxLength200(repr))
                .Apply((_, _) => new Description(repr)).As().Map(Some)

        };
    }
    public string To()
    {
        return Repr;
    }

    public static Description FromUnsafe(string repr)
    {
        return new Description(repr);
    }
};