using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

using Domain.Amenities.ValueObjects;
using Domain.Apartments.ValueObjects;
using Domain.Bookings.ValueObjects;


using Description = Domain.Amenities.ValueObjects.Description;
using Name = Domain.Amenities.ValueObjects.Name;
using State = Domain.Amenities.ValueObjects.State;

namespace Domain.Amenities;

public record Amenity :
    IEqualityOperators<Amenity, Amenity, bool>,
    IEqualityOperators<Amenity, (string Name, string Description, string State, double? Cost, double? Percentage), bool>
{
    public State State { get; }
    public Name Name { get; }

    private string? _description;
    private double? _percentage;
    private double? _costPauschal;

    [NotMapped]
    public Option<Description> Description
    {
        get => OptionFromUnsafe<Description, string>(_description);
        set => _description = value.NullIfNoneString<Description, string>();
    }
    [NotMapped]
    public Option<Money> CostPauschal
    {
        get => Money.FromNullable(_costPauschal);
        set => _costPauschal = value.Match<double?>(m => m.Value, () => null);
    }
    [NotMapped]
    public Option<Percentage> Percentage
    {
        get => OptionFromUnsafe<Percentage, double>(_percentage);
        set => _percentage = value.Match<double?>(m => m.Repr, () => null);
    }

    private Amenity() { }
    private Amenity(Name name, Option<Description> description, State state, Option<Money> costPauschal, Option<Percentage> percentage)
    {
        Name = name;
        Description = description;
        State = state;
        CostPauschal = costPauschal;
        Percentage = percentage;
    }


    public static Fin<Amenity> Create(string name, string? description, string state, double? cost, double? percentage)
    {
        return (
                Name.From(name),
                FromNullable<Description, string>(description),
                State.From(state),

                FromNullable<Percentage, double>(percentage)
                )
               .Apply((n, d, s, p) =>
                   new Amenity(n, d, s, Money.FromNullable(cost), p)).As();
    }
    public (string Name, string? Description, string State, double? Cost, double? Perccentage) To()
    {
        return (
            Name.To(),
            Description.Match<string?>(description => description.To(), () => null),
            State.Slug,
            CostPauschal.Match<double?>(money => money.To().Value, () => null),
            NullIfNone<Percentage, double>(Percentage)

            );
    }


    public Fin<Money> CalculateCost(DateRange dateRange)
    {
        return (Percentage, CostPauschal) switch
        {
            ({ IsNone: true }, { IsNone: true }) => Money.Zero,
            ({ IsNone: true }, { IsNone: false }) => CostPauschal.Match(money => money, () => Money.Zero),
            ({ IsNone: false }, { IsNone: true }) => Percentage.Match(percentage => Money.FromDouble(percentage.To() * dateRange.DurationLengthInDays), () => Money.Zero),
            _ => FinFail<Money>(ValidationErrors.Domain.Amenity.Cost.Invalid($"Amenity {Name.Slug} should have a pauschal cost or percentage applied for period, not both."))
        };
    }
    public static bool operator ==(Amenity? left, (string Name, string Description, string State, double? Cost, double? Percentage) right)
    {
        return left is { } a && a.To() == right;
    }

    public static bool operator !=(Amenity? left, (string Name, string Description, string State, double? Cost, double? Percentage) right)
    {
        return !(left == right);
    }



}