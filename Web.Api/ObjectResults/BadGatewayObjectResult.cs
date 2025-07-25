using Microsoft.AspNetCore.Mvc;

namespace Web.Api.ObjectResults;

public class BadGatewayObjectResult : ObjectResult
{
    public BadGatewayObjectResult(object value)
        : base(value)
    {
        StatusCode = StatusCodes.Status502BadGateway;
    }
}