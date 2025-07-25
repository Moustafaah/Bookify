using LanguageExt.Common;

namespace Domain.Shared.Errors;

public record NotFoundError : Error
{
    private NotFoundError()
    {
        IsEmpty = true;
    }
    private NotFoundError(string message)
    {
        IsEmpty = false;
        Message = message;
    }

    public new static NotFoundError New(string message) => new(message);
    public override string Message { get; }


    public override int Code => 404;

    public override bool IsExceptional => false;
    public override bool IsExpected => true;


    public override ErrorException ToErrorException()
    {
        return ErrorException.New(Code, Message);
    }

    public override bool IsEmpty { get; }

    public static NotFoundError Empty => new NotFoundError();
}