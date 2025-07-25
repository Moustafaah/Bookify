using Application.Messaging;

namespace Application.Users.Queries;

public record GetUserByIdQuery(string UserId) : IQuery<UserViewModel>;