using Microsoft.AspNetCore.Mvc;

namespace Web.Api.ObjectResults;

public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object value)
        : base(value)
    {
        StatusCode = StatusCodes.Status403Forbidden;
    }
}