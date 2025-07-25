using Microsoft.AspNetCore.Mvc;

namespace Web.Api.ObjectResults;

public class TooManyRequestsObjectResult : ObjectResult
{
    public TooManyRequestsObjectResult(object value)
        : base(value)
    {
        StatusCode = StatusCodes.Status429TooManyRequests;
    }
}