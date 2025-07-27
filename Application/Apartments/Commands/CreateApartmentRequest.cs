using Application.Messaging;

namespace Application.Apartments.Commands;

public sealed record CreateApartment(
    string Name,
    string? Description,
    double Price,
    string CurrencyCode,
    double? CleaningFee,
    IEnumerable<CreateAmenityRequest> Amenities) : ICommand<Guid>;
