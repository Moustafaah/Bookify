using Application.Messaging;

namespace Application.Apartments.Commands;
public record DeleteApartment(Guid Id) : ICommand<Unit>
{
}
