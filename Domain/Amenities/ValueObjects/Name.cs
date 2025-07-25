using LanguageExt.Traits.Domain;

namespace Domain.Amenities.ValueObjects;

public sealed record Name : DomainType<Name, string>
{
    public string Slug { get; }
    public int Value { get; }

    private static readonly List<Name> _all = new();

    private Name(int value, string name)
    {
        Value = value;
        Slug = name;
        _all.Add(this);
    }

    public static readonly Name Pool = new(1, "Pool");
    public static readonly Name Gym = new(2, "Gym");
    public static readonly Name Spa = new(3, "Spa");
    public static readonly Name Wifi = new(4, "Wi-Fi");
    public static readonly Name Parking = new(5, "Parking");
    public static readonly Name Restaurant = new(6, "Restaurant");
    public static readonly Name Bar = new(7, "Bar");
    public static readonly Name Laundry = new(8, "Laundry");
    public static readonly Name Concierge = new(9, "Concierge");
    public static readonly Name AirConditioning = new(10, "Air Conditioning");
    public static readonly Name Heating = new(11, "Heating");
    public static readonly Name RoomService = new(12, "Room Service");
    public static readonly Name Housekeeping = new(13, "Housekeeping");
    public static readonly Name AirportShuttle = new(14, "Airport Shuttle");
    public static readonly Name BusinessCenter = new(15, "Business Center");
    public static readonly Name ConferenceRoom = new(16, "Conference Room");
    public static readonly Name NonSmoking = new(17, "Non-Smoking Rooms");
    public static readonly Name PetFriendly = new(18, "Pet-Friendly");
    public static readonly Name HourFrontDesk24 = new(19, "24-Hour Front Desk");
    public static readonly Name SafeDepositBox = new(20, "Safe Deposit Box");
    public static readonly Name Elevator = new(21, "Elevator");
    public static readonly Name BreakfastIncluded = new(22, "Breakfast Included");
    public static readonly Name MiniBar = new(23, "Mini Bar");
    public static readonly Name CoffeeMaker = new(24, "Coffee Maker");
    public static readonly Name HairDryer = new(25, "Hair Dryer");
    public static readonly Name IroningFacilities = new(26, "Ironing Facilities");
    public static readonly Name Television = new(27, "Television");
    public static readonly Name Balcony = new(28, "Balcony");
    public static readonly Name SeaView = new(29, "Sea View");


    public static IReadOnlyList<Name> All => _all;

    public override string ToString() => Slug;

    public static Fin<Name> FromValue(int value) =>
        _all.FirstOrDefault(a => a.Value == value) is { } am
            ? FinSucc(am)
            : FinFail<Name>(ValidationErrors.Domain.Amenity.Name.Invalid(value));


    public static Fin<Name> FromName(string name) =>
        _all.FirstOrDefault(a => string.Equals(a.Slug, name, StringComparison.OrdinalIgnoreCase)) is { } am
            ? FinSucc(am)
            : FinFail<Name>(ValidationErrors.Domain.Amenity.Name.Invalid(name));

    public static Fin<Name> From(string repr) => ValidateName(repr).Map(s =>
    {
        var x = new Name(_all.Count, repr);
        _all.Add(x);
        return x;
    });



    public string To()
    {
        return Slug;
    }

    private static Fin<Unit> ValidateName(string repr)
    {
        return from _ in IsNullOrEmpty(repr).Bind(_ => IsNullOrWhiteSpace(repr))
               from __ in AmenityNameExists(repr)
               select unit;

    }

    private static Fin<Unit> AmenityNameExists(string repr)
    {
        return _all.FirstOrDefault(a => string.Equals(a.Slug, repr, StringComparison.OrdinalIgnoreCase)) is { } am
            ? FinFail<Unit>(ValidationErrors.Domain.Amenity.Name.AlreadyExists(repr))
            : unit;
    }

    public static Name FromUnsafe(string repr)
    {
        return _all.FirstOrDefault(a => a.Slug == repr) is { } am ? am : new Name(_all.Count, repr);
    }
}