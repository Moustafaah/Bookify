namespace Domain.Apartments.ValueObjects;

public record Money

//Amount<Money, int>, // as cents
//Amount<Money, double> // as money value
{

    public int Dollar { get; }
    public int Cent { get; }
    public double Value { get; }

    public static Money Zero => FromDouble(0);
    private Money() { }
    private Money(int dollar, int cent, double value)
    {
        Dollar = dollar;
        Cent = cent;
        Value = value;
    }

    public static Money FromDouble(double repr)
    {
        var d = repr * 100;
        var m = d / 100;
        var l = d % 100;
        return new Money((int)m, (int)l, repr);
    }


    public static Option<Money> FromNullable(double? repr)
    {
        return Optional(repr).Map(FromDouble);
    }


    public (int Doller, int Cent, double Value) To()
    {
        return (Dollar, Cent, Value);
    }

    public static Money operator -(Money value)
    {
        return FromDouble(-value.Value);
    }

    public static Money operator +(Money left, Money right)
    {

        return FromDouble(left.Value + right.Value);

    }

    public static Money Convert(Money from, Currency toCurrency)
    {
        return FromDouble(from.Value * toCurrency.To().ConversionRate);
    }

    private static Money FromCents(int cent)
    {
        var m = cent / 100;
        var l = cent % 100;

        (int d, int c) t = l switch
        {
            < 0 => (m - 1, l + 100),
            _ => (m, l)
        };

        var value = double.Parse($"{t.d},{t.c}");

        return new Money(t.d, t.c, value);
    }

    public static Money operator -(Money left, Money right)
    {
        return FromDouble(
              left.Value - right.Value);

    }



    public int CompareTo(Money other)
    {
        return To().Value.CompareTo(other.To().Value);
    }

    public static bool operator >(Money left, Money right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Money left, Money right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(Money left, Money right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Money left, Money right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static Money operator *(Money left, int right)
    {

        var total = left.Value * right;
        return FromDouble(total);
    }

    public static Money operator /(Money left, int right)
    {

        var total = left.Value / right;

        return FromDouble(total);
    }

    public static Money operator *(Money left, double right)
    {
        var total = left.Value * right;
        return FromDouble(total);
    }

    public static Money operator /(Money left, double right)
    {
        var total = left.Value / right;
        return FromDouble(total);
    }



}