namespace Web.Api.Requests.User;

public class CreateUserRequest(string firstname, string lastname, string email)
{
    public string Firstname { get; } = firstname;
    public string Lastname { get; } = lastname;
    public string Email { get; } = email;
}