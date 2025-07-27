namespace Application.Apartments.Commands;
public class CreateAmenityRequest
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public string State { get; init; }
    public double? Cost { get; init; }
    public double? Percentage { get; init; }
}