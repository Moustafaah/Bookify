using Application.Booking;

using Domain.Apartments;
using Domain.Apartments.ValueObjects;

namespace Application.Apartments;
public class ApartmentViewModel
{

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public double Price { get; init; }
    public string Currency { get; init; }

    public double? CleaningFee { get; init; }

    public DateTime? LastBookedAt { get; init; }

    public IEnumerable<AmenityViewModel> Amenities { get; init; }

    public IEnumerable<BookingViewModel> Bookings { get; init; }

}

public static partial class ViewModel
{
    public static ApartmentViewModel ToViewModel(this Apartment apartment)
    {
        return new ApartmentViewModel()
        {
            Id = apartment.Id,
            Name = apartment.Name.To(),
            Price = apartment.Price.Value,
            Currency = apartment.Currency.Code,
            Description = apartment.Description.NullIfNoneString<Description, string>(),
            CleaningFee = apartment.CleaningFee.Match<double?>(money => money.Value, () => null),
            LastBookedAt = NullIfNone(apartment.LastBookedAt),
            Amenities = apartment.Amenities.Map(amenity => amenity.ToViewModel()),
            Bookings = apartment.Bookings.Map(booking => booking.ToViewModel())

        };
    }
}