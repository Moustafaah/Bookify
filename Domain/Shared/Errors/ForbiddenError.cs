using LanguageExt.Common;

namespace Domain.Shared.Errors;

public record ForbiddenError : Error
{
    private ForbiddenError()
    {
        IsEmpty = true;
    }
    private ForbiddenError(string message)
    {
        IsEmpty = false;
        Message = message;
    }

    public new static ForbiddenError New(string message) => new(message);
    public override string Message { get; }


    public override int Code => 400;

    public override bool IsExceptional => false;
    public override bool IsExpected => true;


    public override ErrorException ToErrorException()
    {
        return ErrorException.New(Code, Message);
    }

    public override bool IsEmpty { get; }

    public static ForbiddenError Empty => new ForbiddenError();

}