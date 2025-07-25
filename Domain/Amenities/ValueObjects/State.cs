using System.Numerics;

using LanguageExt.Traits.Domain;

namespace Domain.Amenities.ValueObjects;



public sealed record State : DomainType<State, string>,
    IEqualityOperators<State, (int Value, string Name), bool>
{
    public int Value { get; }
    public string Slug { get; }


    private static readonly List<State> _all = new();

    private State(int value, string name)
    {
        Value = value;
        Slug = name;

        _all.Add(this);
    }

    public static readonly State BrandNew = new(1, "Brand New");
    public static readonly State LikeNew = new(2, "Like New");
    public static readonly State GentlyUsed = new(3, "Gently Used");
    public static readonly State Used = new(4, "Used");
    public static readonly State Worn = new(5, "Worn");
    public static readonly State Damaged = new(6, "Damaged");
    public static readonly State NeedsRepair = new(7, "Needs Repair");
    public static readonly State OutOfOrder = new(8, "Out of Order");
    public static readonly State Decommissioned = new(9, "Decommissioned");

    public static IReadOnlyList<State> All => _all;

    public override string ToString() => Slug;

    public static Fin<State> FromValue(int value) =>
        _all.FirstOrDefault(s => s.Value == value) is { } a
            ? FinSucc(a)
            : FinFail<State>(ValidationErrors.Domain.Amenity.State.Invalid(value));


    public static Fin<State> FromValue(string name) =>
        _all.FirstOrDefault(s => string.Equals(s.Slug, name, StringComparison.OrdinalIgnoreCase)) is { } a
            ? FinSucc(a)
            : FinFail<State>(ValidationErrors.Domain.Amenity.State.Invalid(name));

    public static bool operator ==(State? left, (int Value, string Name) right)
    {
        return left is { } l && (l.Value, l.Slug) == right;
    }

    public static bool operator !=(State? left, (int Value, string Name) right)
    {
        return !(left == right);
    }


    public static Fin<State> From(string repr)
    {
        return ValidateName(repr).Map(s =>
        {
            var x = new State(_all.Count, repr);
            _all.Add(x);
            return x;
        });
    }

    public string To()
    {
        return Slug;
    }



    private static Fin<Unit> ValidateName(string repr)
    {
        return from _ in IsNullOrEmpty(repr)
                .Bind(s => IsNullOrWhiteSpace(repr))
               from __ in AmenityStateExists(repr)
               select unit;

    }

    private static Fin<Unit> AmenityStateExists(string repr)
    {
        return _all.FirstOrDefault(a => string.Equals(a.Slug, repr, StringComparison.OrdinalIgnoreCase)) is { } am
            ? FinFail<Unit>(
                ValidationErrors.Domain.Amenity.Name.AlreadyExists(repr))
            : unit;
    }

    public static State FromUnsafe(string repr)
    {
        return _all.FirstOrDefault(a => a.Slug == repr) is { } am ? am : new State(_all.Count, repr);
    }


}