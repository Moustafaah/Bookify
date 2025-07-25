using Application.Messaging;

using Domain.Monads.Db;
using Domain.Shared.Errors;
using Domain.Users;
using Domain.Users.Events;

using Infrastructure.Data.Repositories;
using Infrastructure.DbRuntime;
using Infrastructure.Monads.Db;

namespace Application.Users.Commands;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Fin<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return (await (from u in User.Create(request.Firstname, request.Lastname, request.Email)
                       from b in UserRepo.GetUserExistsByEmail(u.Email.Repr)
                       from g in b
                           ? Db<BookifyRT>.fail<User>(ConflictError.New($"User with email: {u.Email.Repr} already exists"))
                               .As()
                           : UserRepo.AddUser(u).RaiseDomainEvent(user => new UserCreatedDomainEvent(user.Id))
                       select g.Id)
                .RunSaveAsync(EnvIO.New(null, cancellationToken)));



    }





}