using Application.Messaging;

namespace Application.Users.Commands;

public record CreateUserCommand(string Firstname, string Lastname, string Email) : ICommand<Guid>;