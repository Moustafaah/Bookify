using Application.Messaging;

using Domain.Amenities;

using Infrastructure.Data.Repositories;
using Infrastructure.Monads.Db;

using LanguageExt.Common;
using LanguageExt.Traits;

using static Domain.Apartments.Apartment;

namespace Application.Apartments.Commands;
internal sealed class CreateApartmentHandler : ICommandHandler<CreateApartment, Guid>
{
    public async Task<Fin<Guid>> Handle(CreateApartment request, CancellationToken cancellationToken)
    {
        var amenities = toSeq(request.Amenities)
            .Traverse(a => Amenity.Create(a.Name, a.Description, a.State, a.Cost, a.Percentage)).As();

        var apartment = Create(
             request.Name,
             request.Description,
             request.Price,
             request.CurrencyCode,
             request.CleaningFee);

        var ap = (apartment, amenities).Apply(((a, s) => a.AddAmenities(s))).As();



        return (await (from a in ap
                       from g in ApartmentRepo.AddApartment(a)
                       select g).RunSaveAsync(EnvIO.New(null, cancellationToken)));
    }




}







public interface IResult<E, A> where E : Monoid<E>
{
    public abstract E Fail { get; }
    public abstract A Succ { get; }
    //public static abstract  IResult<E, A> Success(A value);
    //public static abstract IResult<E, A> Failure(E e);




}


public class FinResult<A> : IResult<Error, A>
{
    public Error Fail { get; }
    public A Succ { get; }
    private FinResult(Error e)
    {
        Fail = e;
        Succ = default;
    }

    private FinResult(A v)
    {
        Fail = default;
        Succ = v!;
    }




    public static IResult<Error, A> Success(A value)
    {
        return new FinResult<A>(value);
    }

    public static IResult<Error, A> Failure(Error e)
    {
        return new FinResult<A>(e);
    }


}



public class ValidationResult<E, A> : IResult<E, A> where E : Error, Monoid<E>, Semigroup<E>
{

    private ValidationResult(E e)
    {
        Fail = e;
        Succ = default;
    }

    private ValidationResult(A v)
    {
        Fail = default;
        Succ = v!;
    }
    public E Fail { get; }
    public A Succ { get; }

    public static IResult<E, A> Success(A value)
    {
        return new ValidationResult<E, A>(value);
    }

    public static IResult<E, A> Failure(E e)
    {
        return new ValidationResult<E, A>(e);
    }



}