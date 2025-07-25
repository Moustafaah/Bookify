using LanguageExt.Common;

namespace Domain.Shared.Errors;

public record ConflictError : Error
{
    private ConflictError()
    {
        IsEmpty = true;
    }
    private ConflictError(string message)
    {
        IsEmpty = false;
        Message = message;
    }

    public new static ConflictError New(string message) => new(message);
    public override string Message { get; }


    public override int Code => 409;

    public override bool IsExceptional => false;
    public override bool IsExpected => true;


    public override ErrorException ToErrorException()
    {
        return ErrorException.New(Code, Message);
    }

    public override bool IsEmpty { get; }

    public static ConflictError Empty => new ConflictError();

}