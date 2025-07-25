using Domain.Monads.Db;
using Domain.Shared.Errors;
using Domain.Users;

using Infrastructure.DbRuntime;
using Infrastructure.Monads.Db;

using Microsoft.EntityFrameworkCore;

using DbExtensions = Infrastructure.Monads.Db.DbExtensions;

namespace Infrastructure.Data.Repositories;
public static class UserRepo
{
    public static Db<BookifyRT, User> GetUserById(Guid id) =>
        from u in Db<BookifyRT>.liftVIO(async (rt, e) => await rt.DbContext.Users.FindAsync([id], e.Token))

        from _ in DbExtensions.As(when(u is null, Db<BookifyRT>.fail<Unit>(NotFoundError.New($"User with id: '{id}' was not found"))))
        select u;

    public static Db<BookifyRT, bool> GetUserExistsByEmail(string email) =>
        from u in Db<BookifyRT>.liftVIO(async (rt, e) => await rt.DbContext.Users.FirstOrDefaultAsync(user => user.Email.Repr == email, e.Token))
        select u is not null;


    public static Db<BookifyRT, User> GetUserByEmail(string email) =>
        from u in Db<BookifyRT>.liftVIO(async (rt, e) => await rt.DbContext.Users.FirstOrDefaultAsync(user => user.Email.Repr == email, e.Token))
        from _ in DbExtensions.As(when(u is null, Db<BookifyRT>.fail<Unit>(NotFoundError.New($"User with email: '{email}' was not found"))))
        select u;

    public static Db<BookifyRT, User> AddUser(User user) =>
        from u in Db<BookifyRT>.lift((rt) => rt.DbContext.Users.Add(user))
        select u.Entity;

    public static Db<BookifyRT, Unit> DeleteUser(Guid id) =>
        from u in GetUserById(id)
        from e in Db<BookifyRT>.lift(rt => rt.DbContext.Users.Remove(u))
        select unit;

    public static Db<BookifyRT, User> UpdateUser(User user) =>
        from u in GetUserById(user.Id)
        select u.Update(user);
}



