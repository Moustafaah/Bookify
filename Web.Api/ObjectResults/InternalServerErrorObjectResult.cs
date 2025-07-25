using Microsoft.AspNetCore.Mvc;

namespace Web.Api.ObjectResults;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value)
        : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}