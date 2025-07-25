using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

using Domain.Abstraction;
using Domain.Bookings;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed record User : Entity, IEqualityOperators<User, Guid, bool>
{


    public Firstname Firstname { get; private set; }
    public Lastname Lastname { get; private set; }
    public Email Email { get; private set; }


    [NotMapped]
    public Seq<Booking> Bookings => toSeq(_bookings);
    private ICollection<Booking> _bookings { get; } = [];
    private User() : base(Guid.NewGuid()) { }
    private User(Guid Id, Firstname firstname, Lastname lastname, Email email) : base(Id)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
    }

    public static Fin<User> Create(string firstname, string lastname, string email)
    {
        return (Firstname.From(firstname), Lastname.From(lastname),
                Email.From(email)).Apply((fn, ln, e) => new User(Guid.NewGuid(), fn, ln, e))
            .As();
    }

    public static User FromUnsafe(Guid id, string firstname, string lastname, string email)
    {
        return new User(id, Firstname.FromUnsafe(firstname), ValueObjects.Lastname.FromUnsafe(lastname),
            Email.FromUnsafe(email));
    }


    public User Update(User user)
    {
        return this with
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
        };
    }


    public bool Equals(User? other)
    {
        return other is { } o && Id.Equals(o.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Firstname.Repr, Lastname.Repr, Email.Repr);
    }
    public (Guid Id, string Firstname, string Lastname, string Email) To()
    {
        return (Id, Firstname.To(), Lastname.To(), Email.To());
    }
    public static bool operator ==(User? left, Guid right)
    {
        return left is { } l && l.Id.Equals(right);
    }

    public static bool operator !=(User? left, Guid right)
    {
        return !(left == right);
    }

}

