using Application.Messaging;

namespace Application.Apartments.Commands;
public record DeleteApartmentRequest(Guid Id) : ICommand<Unit>
{
}
