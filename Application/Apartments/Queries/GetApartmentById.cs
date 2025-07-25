using Application.Messaging;

namespace Application.Apartments.Queries;
internal record GetApartmentById(Guid Id) : IQuery<ApartmentViewModel>
{
}
