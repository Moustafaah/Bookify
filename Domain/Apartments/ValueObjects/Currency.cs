using System.Numerics;

namespace Domain.Apartments.ValueObjects;



public record Currency :
    IEqualityOperators<Currency, string, bool>
{
    public string Code { get; }
    public string Name { get; }
    public double ConversionRate { get; }

    private Currency(string code)
    {
        Code = code;
    }
    public bool IsNone => this.To() == None.To();
    private static readonly List<Currency> _all = new();


    private Currency(string code, string name, double conversionRate)
    {
        Code = code;
        Name = name;
        ConversionRate = conversionRate;
        _all.Add(this);
    }

    // Predefined currencies
    public static readonly Currency None = new("None", "No Currency Specified", 0);
    public static readonly Currency USD = new("USD", "US Dollar", 1.0);
    public static readonly Currency EUR = new("EUR", "Euro", 0.92);
    public static readonly Currency GBP = new("GBP", "British Pound", 0.78);
    public static readonly Currency JPY = new("JPY", "Japanese Yen", 110.25);
    public static readonly Currency CAD = new("CAD", "Canadian Dollar", 1.36);
    public static readonly Currency AUD = new("AUD", "Australian Dollar", 1.48);
    public static readonly Currency CHF = new("CHF", "Swiss Franc", 0.89);


    public static IReadOnlyList<Currency> All => _all;

    public override string ToString() => Code;

    public static Fin<Currency> FromCode(string code) =>
        _all.FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase)) is { } c
            ? FinSucc(c)
            : FinFail<Currency>(ValidationErrors.Domain.Currency.Invalid(code, nameof(Currency))); // TODO

    public static Fin<Currency> FromName(string name) =>
        _all.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)) is { } c
            ? FinSucc<Currency>(c)
            : FinFail<Currency>(ValidationErrors.Domain.Currency.Invalid(name, nameof(Currency))); // TODO

    public static bool operator ==(Currency? left, string? right) =>
        left is { } l && right is { } r && l.To().Code == right;

    public static bool operator !=(Currency? left, string? right) =>
        !(left == right);

    public static Currency FromUnsafe(string code, string name, double conversionRate) =>
        new Currency(code.ToUpperInvariant(), name, conversionRate);



    public (string Code, string Name, double ConversionRate) To() =>
        (Code, Name, ConversionRate);


    public static Fin<Option<Currency>> FromNullable(string? currencyCode)
    {
        return currencyCode is { } ? FromCode(currencyCode).Map(Some) : Option<Currency>.None;
    }

    public static Currency FromUnsafe(string repr)
    {
        return _all.FirstOrDefault(c => c.Code == repr) is { } cu ? cu : None;
    }
}