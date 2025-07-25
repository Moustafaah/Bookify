using LanguageExt.Traits.Domain;

namespace Domain.Amenities.ValueObjects;
public class Percentage : DomainType<Percentage, double>
{
    public readonly double Repr;

    private Percentage(double repr)
    {
        Repr = repr;
    }
    public static Fin<Percentage> From(double repr)
    {
        return repr > .50 || repr <= 0.0
            ? FinFail<Percentage>(ValidationErrors.Domain.Amenity.Percentage.Invalid(
                $"Percentage cannot be more than '50%' or less than or equal to '0%'"))
            : new Percentage(repr);
    }

    public double To()
    {
        return Repr;
    }

    public static Percentage FromUnsafe(double repr)
    {
        return new Percentage(repr);
    }
}
