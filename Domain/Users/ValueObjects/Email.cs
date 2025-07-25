using System.Numerics;
using System.Text.RegularExpressions;

using LanguageExt.Traits.Domain;

namespace Domain.Users.ValueObjects;
public record Email : DomainType<Email, string>,
    IEqualityOperators<Email, string, bool>
{
    public readonly string Repr;

    private Email(string repr)
    {
        Repr = repr;
    }
    public static Fin<Email> From(string repr)
    {
        return IsValidEmail(repr)
            ? FinSucc(new Email(repr))
            : FinFail<Email>(ValidationErrors.Domain.Users.Email.Invalid(repr));
    }



    public string To()
    {
        return Repr;
    }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);

    public static bool operator ==(Email? left, string? right)
    {
        return left is { } e && right is { } r && String.Equals(e.Repr, r, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator !=(Email? left, string? right)
    {
        return !(left == right);
    }

    public static Email FromUnsafe(string repr)
    {
        return new Email(repr);
    }
}
