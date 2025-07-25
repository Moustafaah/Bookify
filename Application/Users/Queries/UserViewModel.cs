using Domain.Users;

namespace Application.Users.Queries;

public class UserViewModel
{
    public string Email { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
}

public static partial class ViewModel
{
    public static UserViewModel ToViewModel(this User user)
    {
        return new UserViewModel()
        { FirstName = user.Firstname.To(), LastName = user.Lastname.To(), Email = user.Email.To() };
    }
}