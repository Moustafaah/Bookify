using System.Text.RegularExpressions;

using LanguageExt;
using LanguageExt.Common;

using Microsoft.AspNetCore.Mvc;

using Web.Api.ObjectResults;
using Web.Api.StatusCode;

namespace Web.Api.Extensions;

public static class FinExtensions
{

    private static readonly Regex DuplicateIndexAndEmailRegex = new(
        @"'IX_[^_]+_(\w+)'.*? \(([^)]+)\)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
    );
    public static string ExtractFields(string message)
    {
        var match = DuplicateIndexAndEmailRegex.Match(message);

        return $"{match.Groups[1].Value.ToUpperInvariant()}  {match.Groups[2].Value} already exists";

    }


    public static ActionResult GetFailActionResult(Error error, string route)
    {
        var list = new List<string>();
        HttpStatusCodeInfo codeInfo;


        switch (error)
        {
            case ManyErrors me:
                list.AddRange(me.Errors.Select(e => e.Message));
                codeInfo = HttpStatusCodeInfo.FromCode(me.Errors.First().Code);
                break;

            case Expected ex:
                list.Add(ex.Message);
                codeInfo = HttpStatusCodeInfo.FromCode(ex.Code);
                break;

            //case Exceptional { Code: -2146233088, Inner.IsSome: true } ex when ex.Inner.ValueUnsafe()!.Code == -2146232060:
            //    list.Add(ex.Inner.Match<string>(e => ExtractFields(e.Message), () => ex.Message));
            //    codeInfo = HttpStatusCodeInfo.FromCode(-2146232060);
            //    break;

            case Exceptional ex:
                list.Add(ex.Message);
                codeInfo = HttpStatusCodeInfo.FromCode(ex.Code);
                break;

            default:
                codeInfo = HttpStatusCodeInfo.FromCode(StatusCodes.Status500InternalServerError);
                list.Add("An internal server error happened, please try again later.");
                break;
        }

        var details = list.ToProblemDetails(codeInfo, route);

        return codeInfo.Code switch
        {
            StatusCodes.Status400BadRequest => new BadRequestObjectResult(details),
            StatusCodes.Status401Unauthorized => new UnauthorizedObjectResult(details),
            StatusCodes.Status403Forbidden => new ForbiddenObjectResult(details),
            StatusCodes.Status404NotFound => new NotFoundObjectResult(details),
            StatusCodes.Status409Conflict => new ConflictObjectResult(details),
            StatusCodes.Status422UnprocessableEntity => new UnprocessableEntityObjectResult(details),
            StatusCodes.Status429TooManyRequests => new TooManyRequestsObjectResult(details),
            StatusCodes.Status502BadGateway => new BadGatewayObjectResult(details),
            StatusCodes.Status500InternalServerError => new InternalServerErrorObjectResult(details),
            //-2146232060 => new ConflictObjectResult(details),
            _ => new InternalServerErrorObjectResult(details)
        };




    }


    public static ProblemDetails ToProblemDetails(this IEnumerable<string> @this, HttpStatusCodeInfo codeInfo, string route)
    {


        return new ProblemDetails()
        {
            Title = codeInfo.Name,
            Status = codeInfo.Code,
            Type = codeInfo.TypeUri,
            Detail = codeInfo.Description,
            Instance = route,
            Extensions = new Dictionary<string, object?> { { "Errors", @this } }
        };
    }

    public static ActionResult<T> ToActionResult<T>(
        this Fin<T> ma, Func<T, ActionResult<T>> succ,
        Func<Error, ActionResult<T>> fail)
    {
        return ma.Match(succ, e => fail(e));
    }






}