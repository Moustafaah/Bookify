using Domain.Amenities;

namespace Application.Apartments.Commands;
public record UpdateApartment(string Name,
    string? Description,
    double Price,
    string CurrencyCode,
    double? CleaningFee,
    IEnumerable<Amenity> Amenities)
{
}
