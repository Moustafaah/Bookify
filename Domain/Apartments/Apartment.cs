using System.ComponentModel.DataAnnotations.Schema;

using Domain.Abstraction;
using Domain.Amenities;
using Domain.Apartments.ValueObjects;
using Domain.Bookings;

using LanguageExt.UnsafeValueAccess;

namespace Domain.Apartments;

using static ValidationHelpers;

public sealed record Apartment : Entity
{
    private double? _cleaningFee;
    private DateTime? _lastBookedAt;
    private string? _description;
    public Name Name { get; private set; }

    public Money Price { get; private set; }
    public Currency Currency { get; private set; }

    public Seq<Amenity> Amenities { get; private set; } = [];
    public Seq<Booking> Bookings { get; private set; } = [];


    [NotMapped]
    public Option<DateTime> LastBookedAt
    {
        get => Optional(_lastBookedAt);
        set => _lastBookedAt = value.ValueUnsafe();
    }
    [NotMapped]
    public Option<Description> Description
    {
        get => OptionFromUnsafe<Description, string>(_description);
        set => _description = value.NullIfNoneString<Description, string>();
    }
    [NotMapped]
    public Option<Money> CleaningFee
    {
        get => Money.FromNullable(_cleaningFee);
        set => _cleaningFee = value.Match<double?>(m => m.Value, () => null);
    }

    private Apartment() : base(Guid.NewGuid()) { }

    private Apartment(
        Name name,
        Option<Description> description,
        Money price,
        Option<Money> cleaningFee,
        Currency currency
      ) : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        Price = price;
        CleaningFee = cleaningFee;
        Currency = currency;
    }


    public static Fin<Apartment> Create(
        string name,
        string? description,
        double price,
        string currencyCode,
        double? cleaningFee)
    {
        return (
                Name.From(name),
                FromNullable<Description, string>(description),
                Currency.FromCode(currencyCode)
            ).Apply((n, d, c)
                => new Apartment(
                    n,
                    d,
                    Money.FromDouble(price),
                    Money.FromNullable(cleaningFee),
                    c)).As();

    }

    public bool HasBookings()
    {
        return Bookings.Any();
    }

    public Apartment AddAmenities(Seq<Amenity> amenities)
    {
        return this with
        {
            Amenities = Amenities.Append(amenities)
        };
    }

    public Unit ChangeLastBookedAt(DateTime dateTime)
    {
        LastBookedAt = Some(dateTime);
        return unit;
    }

    public Apartment AddAmenities(Amenity amenity)
    {
        return this with
        {
            Amenities = Amenities.Add(amenity)
        };
    }

    public Apartment DeleteAmenities(Seq<Amenity> amenities)
    {
        Amenities = Amenities.Filter(a => amenities.Exists(b => a.Name.Value == b.Name.Value));
        return this;
    }
    public Apartment DeleteAmenities(Amenity amenity)
    {

        return this with
        {
            Amenities = Amenities.Filter(a => a.Name.Value == amenity.Name.Value)
        };


    }

    public Apartment AddBooking(Booking booking)
    {
        return this with
        {
            Bookings = Bookings.Add(booking)

        };
    }

    public Apartment Update(Apartment apartment)
    {

        return this with
        {
            Name = apartment.Name,
            Description = apartment.Description,
            Price = apartment.Price,
            Currency = apartment.Currency,
            Amenities = apartment.Amenities,
            CleaningFee = apartment.CleaningFee,
            LastBookedAt = apartment.LastBookedAt
        };


    }

    //public Entity.Apartment ToEntity()
    //{
    //    return new Entity.Apartment()
    //    {
    //        Id = Id,
    //        Name = Name.To(),
    //        Description = Description.NullIfNoneString<Description, string>(),
    //        Price = Price.Value,
    //        Currency = Currency.Code,
    //        CleaningFee = CleaningFee.Match<double?>(money => money.Value, () => null),
    //        LastBookedAt = LastBookedAt.Match<DateTime?>(d => d, () => null),
    //        Amenities = Amenities.Map(a =>
    //            new Amenities.Entity.Amenity()
    //            {
    //                Name = a.Name.Slug,
    //                Cost = a.CostPauschal.Match<double?>(money => money.Value, () => null),
    //                Description = a.Description.Match<string?>(d => d.Repr, () => null),
    //                State = a.State.Slug
    //            })

    //    };
    //}

}



//public (Guid Id,
//    string Name,
//    string? Description,
//    (int Doller, int Cent, double Value, string CurrencyCode) Price,
//    (int Doller, int Cent, double Value, string CurrencyCode)? CleaningFee,
//    DateTime? LastBookedAt,
//    IEnumerable<(string Name, string Description, string State, double? Cost, string Currency)> Amenities) To()
//{
//    return (Id, Name.To(),
//        Description.Match<string?>(description => description.To(), () => null),
//        Price.To(),
//        CleaningFee.Match<(int Doller, int Cent, double Value, string CurrencyCode)?>(money => money.To(), () => null),
//        LastBookedAt.Match<DateTime?>(time => time, () => null), Amenities.Select(amenity => amenity.To()));
//}



//public static Apartment FromUnsafe(
//    Guid id,
//    string name,
//    string? description,
//    double price, 
//    string currency, 
//    double cleaningFee, 
//    DateTime lastBookedAt, 
//    IEnumerable<AmenityDto> amenities)
//{
//    return new Apartment(id, ValueObjects.Apartments.Name.FromUnsafe(name), Optional(description).Map(s => ValueObjects.Apartments.Description.FromUnsafe(s)),  )
//}





public record Address(State State, Country Country, ZipCode ZipCode, Street Street)
{
}

public record Street
{
}

public record ZipCode
{
}

public record Country
{
}

public record State
{
}


