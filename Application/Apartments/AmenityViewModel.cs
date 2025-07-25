using Domain.Amenities;

namespace Application.Apartments;
public class AmenityViewModel
{

    public string Name { get; init; }
    public string? Description { get; init; }
    public string State { get; init; }
    public double? CostPauschal { get; init; }
    public double? Percentage { get; init; }

}



public static partial class ViewModel
{
    public static AmenityViewModel ToViewModel(this Amenity amenity)
    {
        return new AmenityViewModel()
        {
            Name = amenity.Name.To(),
            State = amenity.State.Slug,
            CostPauschal = amenity.CostPauschal.Match<double?>(money => money.Value, () => null),
            Description = amenity.Description.Match<string?>(description => description.To(), () => null),
            Percentage = amenity.Percentage.Match<double?>(p => p.To(), () => null),

        };
    }
}

