using Application.Messaging;

using Infrastructure.Data.Repositories;
using Infrastructure.Monads.Db;

namespace Application.Apartments.Queries;
internal sealed class GetApartmentByIdHandler : IQueryHandler<GetApartmentById, ApartmentViewModel>
{
    public async Task<Fin<ApartmentViewModel>> Handle(GetApartmentById request, CancellationToken cancellationToken)
    {
        return (await ApartmentRepo.GetApartmentById(request.Id).RunAsync(EnvIO.New(null, cancellationToken)))
            .Map(apartment => apartment.ToViewModel());
    }


}
