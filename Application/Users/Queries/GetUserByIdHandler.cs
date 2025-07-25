using Application.Messaging;

using Domain.Shared.Errors;

using Infrastructure.Data.Repositories;
using Infrastructure.Monads.Db;

namespace Application.Users.Queries;
internal sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserViewModel>
{
    public Task<Fin<UserViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return (
            from _ in Guid.TryParse(request.UserId, out var id) ? FinSucc(id) : FinFail<Guid>(BadRequestError.New($"Invalid Guid Id format."))
            from u in UserRepo.GetUserById(id)
            select u.ToViewModel()).RunAsync(EnvIO.New(null, cancellationToken));

    }


}


